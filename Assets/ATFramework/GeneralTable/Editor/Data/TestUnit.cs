//-----------------------------------------------------------------------
// <filename>TestUnit</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> ## </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/12/19 星期三 12:11:24# </time>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace GameFramework.Taurus
{
    public struct TestUnit
    {
        public string TestName;
        public string Result;
        public bool enabled;
        public TestUnitState state;

        public TestUnit(string testName, string result)
        {
            TestName = testName;
            Result = result;
            state = TestUnitState.None;
            enabled = false;
        }
    }

    public enum TestUnitState
    {
        Success,
        Fail,
        None
    }

    enum ColumnType
    {
        UnitName,
        Result
    }
}
