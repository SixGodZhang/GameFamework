copy $(TargetDir)Hotfix.dll $(SolutionDir)pdb2mdb /y
copy $(TargetDir)Hotfix.pdb $(SolutionDir)pdb2mdb /y

pushd $(SolutionDir)pdb2mdb
pdb2mdb.exe Hotfix.dll

copy Hotfix.dll $(SolutionDir)Assets\Game\Hotfix\Hotfixdll.bytes /y
copy Hotfix.dll.mdb $(SolutionDir)Assets\Game\Hotfix\Hotfix.dll.mdb.bytes /y