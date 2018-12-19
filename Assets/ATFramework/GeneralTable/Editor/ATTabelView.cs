//-----------------------------------------------------------------------
// <filename>ATTabelView</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> ## </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/12/18 星期二 14:40:59# </time>
//-----------------------------------------------------------------------

using GameFramework.Taurus.UnityEditor;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using static UnityEditor.IMGUI.Controls.MultiColumnHeaderState;

namespace GameFramework.Taurus
{
    public class ATTabelView:TreeView
    {

        List<RowItem<TestUnit>> _allRowDatas;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="state"></param>
        public ATTabelView(TreeViewState state)
            :base(state)
        {
            Reload();
        }

        /// <summary>
        /// 多行构造函数
        /// </summary>
        /// <param name="state"></param>
        /// <param name="multiColumnHeader"></param>
        /// <param name="dataSource"></param>
        public ATTabelView(TreeViewState state, MultiColumnHeader multiColumnHeader, List<TestUnit> dataSource)
            : base(state, multiColumnHeader)
        {
            _allRowDatas = new List<RowItem<TestUnit>>();
            int i = 0;
            foreach (var item in dataSource)
            {
                _allRowDatas.Add(new RowItem<TestUnit>(i++,item));
            }

            showBorder = true;
            rowHeight = EditorGUIUtility.singleLineHeight;

            showAlternatingRowBackgrounds = true;
            multiColumnHeader.sortingChanged += OnSortingChanged;
            multiColumnHeader.visibleColumnsChanged += OnVisibleColumnChanged;

            Reload();
        }

        /// <summary>
        /// 更新表格数据
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="isLoad">是否是加载单元测试</param>
        public void UpdateTableData(List<BaseTestUnit> dataSource, bool isLoad = false)
        {
            _allRowDatas.Clear();
            

            int i = 0;
            foreach (var item in dataSource)
            {
                TestUnit testUnit;
                if (SelectedConstainUnit(item))
                {
                    testUnit = new TestUnit(item.TestName, item.Pass.ToString());
                    testUnit.enabled = true;
                }
                else
                    testUnit = new TestUnit(item.TestName, "--");

                //testUnit.enabled = item.Pass;
                _allRowDatas.Add(new RowItem<TestUnit>(i++, testUnit));
            }

            Reload();
        }

        public bool SelectedConstainUnit(BaseTestUnit unit)
        {
            List<TestUnit> selectedList = GetSelectedUnits();
            if (selectedList == null)
                return false;
            foreach (var item in selectedList)
            {
                if (item.TestName == unit.TestName)
                    return true;

            }

            return false;
        }

        void OnSortingChanged(MultiColumnHeader multiColumnHeader)
        {
            var rows = GetRows();
            SortIfNeeded(rows);

            rows.Clear();
            foreach (var item in rootItem.children)
            {
                rows.Add(item);
            }
            //repaint
            Repaint();
        }

        void SortIfNeeded(IList<TreeViewItem> rows)
        {
            if (rows.Count <= 1 || multiColumnHeader.sortedColumnIndex == -1)
                return;

            // Sort the roots of the existing tree items
            SortByMultipleColumns();

        }


        private bool ascending = true;
        void SortByMultipleColumns()
        {
            var sortedColumns = multiColumnHeader.state.sortedColumns;

            if (sortedColumns.Length == 0)
                return;

            var rowDatas = rootItem.children.Cast<RowItem<TestUnit>>();

            ascending = !ascending;
            var orderedQuery = rowDatas.Order(l => l.Data.TestName, ascending);

            //bool ascending = multiColumnHeader.IsSortedAscending(sortedColumns[1]);
            orderedQuery.ThenBy(l => l.Data.TestName);

            rootItem.children = orderedQuery.Cast<TreeViewItem>().ToList();

        }

        protected override void SingleClickedItem(int id)
        {
            Debug.Log("id: " + id);
            base.SingleClickedItem(id);
        }

        protected override bool CanMultiSelect(TreeViewItem item)
        {
            return true;
        }

        List<TreeViewItem> viewItems = new List<TreeViewItem>();
        protected override IList<TreeViewItem> BuildRows(TreeViewItem root)
        {
            if (rootItem.hasChildren)
                rootItem.children.Clear();
            for (int i = 0; i < _allRowDatas.Count; i++)
            {
                rootItem.AddChild(_allRowDatas[i]);
            }

            //var rows = GetRows();
            viewItems.Clear();

            if (!string.IsNullOrEmpty(searchString))
            {
                Search(searchString, out viewItems);
            }
            else
            {
                viewItems = _allRowDatas.ToList<TreeViewItem>(); 
            }

            return viewItems;
        }

        /// <summary>
        /// 获取选中的单元测试
        /// </summary>
        /// <returns></returns>
        public List<TestUnit> GetSelectedUnits()
        {
            if (!rootItem.hasChildren)
                return null;
            List<TestUnit> selectedUnits = new List<TestUnit>();

            List<RowItem<TestUnit>> items = rootItem.children.Cast<RowItem<TestUnit>>().ToList();
            foreach (var item in items)
            {
                if (item.Data.enabled)
                    selectedUnits.Add(item.Data);
            }

            return selectedUnits;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="rootItem"></param>
        /// <param name="searchString"></param>
        /// <param name="rows"></param>
        private void Search(string searchString, out List<TreeViewItem> rows)
        {
            var itemList = rootItem.children.Cast<RowItem<TestUnit>>();
            IEnumerable<RowItem<TestUnit>> units = itemList.Where(unit =>
            {
                return  unit.Data.TestName.Contains(searchString);
            });

            rows = units.Cast<TreeViewItem>().ToList();
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            var item =(RowItem<TestUnit>)args.item;

            for (var i = 0; i < args.GetNumVisibleColumns(); i++)
                CellGUI(args.GetCellRect(i), item, (ColumnType)args.GetColumn(i), ref args);
        }

        const float kToggleWidth = 18f;
        private void CellGUI(Rect rect, RowItem<TestUnit> item, ColumnType type, ref RowGUIArgs args)
        {
            switch (type)
            {
                case ColumnType.UnitName:
                    {
                        // Do toggle
                        Rect toggleRect = rect;
                        toggleRect.x += GetContentIndent(item);
                        toggleRect.width = kToggleWidth;
                        if (toggleRect.xMax < rect.xMax)
                            item.Data.enabled = EditorGUI.Toggle(toggleRect, item.Data.enabled); // hide when outside cell rect

                        //Do lable
                        Rect labelRect = rect;
                        labelRect.x += toggleRect.x + kToggleWidth;
                        DefaultGUI.Label(labelRect, item.Data.TestName, false, false);
                    }
                    break;
                case ColumnType.Result:
                    {

                        var defaultColor = GUI.color;
                        if (item.Data.Result.ToLower() == "false" )
                            GUI.color = Color.red;
                        else if (item.Data.Result.ToLower() == ("true"))
                            GUI.color = Color.green;
                        else
                            GUI.color = Color.white;
                        DefaultGUI.Label(rect, item.Data.Result, false, false);
                        GUI.color = defaultColor;
                    }
                    break;
            }

        }

        private void OnVisibleColumnChanged(MultiColumnHeader multiColumnHeader)
        {
            throw new NotImplementedException();
        }

        protected override TreeViewItem BuildRoot()
        {
            var root = new TreeViewItem { id = 0, depth = -1, displayName = "Root" };
            return root;
        }

        public static MultiColumnHeaderState CreateDefaultMultiColumnHeaderState()
        {
            var columns = new[]
            {
                new MultiColumnHeaderState.Column
                {
                    headerContent = new GUIContent("TestName"),
                    headerTextAlignment = TextAlignment.Left,
                    sortedAscending = true,
                    sortingArrowAlignment = TextAlignment.Center,
                    width = 600,
                    minWidth = 60,
                    autoResize = false,
                    allowToggleVisibility = false
                },
                new MultiColumnHeaderState.Column
                {
                    headerContent = new GUIContent("Result", "Unit Result"),
                    headerTextAlignment = TextAlignment.Left,
                    sortedAscending = true,
                    sortingArrowAlignment = TextAlignment.Center,
                    width = 110,
                    minWidth = 60,
                    autoResize = true
                }
            };

            Assert.AreEqual(columns.Length, Enum.GetValues(typeof(ColumnType)).Length, "列数不一致");

            var state = new MultiColumnHeaderState(columns);
            return state;
        }
    }
}
