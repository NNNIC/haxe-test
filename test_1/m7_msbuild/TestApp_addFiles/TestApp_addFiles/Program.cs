using System;
using System.Text;
//using Microsoft.Build.BuildEngine;
using Microsoft.Build.Evaluation;

namespace TestApp_addFiles
{
    class Program
    {
        static void Main(string[] args)
        {
            var fullPathName = @"G:\statego\samples\haxe-test\m7_msbuild\TestApp\TestDll\TestDll.csproj";
            var  project = new Microsoft.Build.Evaluation.Project(fullPathName);
            
            //var buildItem = project.AddItem("Content", @"..\..\SomeFunFolder\MyLinkFile.ext");
            //project.Save(fullPathName, Encoding.UTF8);
            
        }
    }
}
