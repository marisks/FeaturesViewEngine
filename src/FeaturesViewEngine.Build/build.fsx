﻿#r @"../../packages/FAKE.4.64.6/tools/FakeLib.dll"

open Fake

let company = "Maris Krivtezs"
let authors = [company]
let projectName = "FeaturesViewEngine"
let projectDescription = "View Engine which enables feature folder support for MVC 5."
let projectSummary = projectDescription
let releaseNotes = "Performance improvements."
let copyright = "Copyright © Maris Krivtezs 2018"
let assemblyVersion = "1.1.6"

let solutionPath = "../../FeaturesViewEngine.sln"
let buildDir = "../FeaturesViewEngine/bin"
let packagesDir = "../../packages/"
let packagingRoot = "../../packaging/"
let packagingDir = packagingRoot @@ "core"
let assemblyInfoPath = "../FeaturesViewEngine/Properties/AssemblyInfo.cs"

let PackageDependency packageName =
    packageName, GetPackageVersion packagesDir packageName

MSBuildDefaults <- {
    MSBuildDefaults with
        Verbosity = Some MSBuildVerbosity.Minimal }

Target "Clean" (fun _ ->
    CleanDirs [buildDir; packagingRoot; packagingDir]
)

open Fake.AssemblyInfoFile

Target "AssemblyInfo" (fun _ ->
    CreateCSharpAssemblyInfo assemblyInfoPath
      [ Attribute.Product projectName
        Attribute.Version assemblyVersion
        Attribute.FileVersion assemblyVersion
        Attribute.ComVisible false
        Attribute.Copyright copyright
        Attribute.Company company
        Attribute.Description projectDescription
        Attribute.Title projectName]
)

let buildMode = getBuildParamOrDefault "buildMode" "Release"

let setParams defaults = {
    defaults with
        Targets = ["Build"]
        Properties =
            [
                "Configuration", buildMode
            ]
    }

Target "BuildApp" (fun _ ->
    build setParams solutionPath
        |> DoNothing
)

Target "CreateCorePackage" (fun _ ->
    let net45Dir = packagingDir @@ "lib/net45/"

    CleanDirs [net45Dir]

    CopyFile net45Dir (buildDir @@ "Release/FeaturesViewEngine.dll")
    CopyFile net45Dir (buildDir @@ "Release/FeaturesViewEngine.pdb")

    NuGet (fun p ->
        {p with
            Authors = authors
            Project = projectName
            Description = projectDescription
            OutputPath = packagingRoot
            Summary = projectSummary
            WorkingDir = packagingDir
            Version = assemblyVersion
            ReleaseNotes = releaseNotes
            Publish = false
            Dependencies =
                [
                PackageDependency "Microsoft.AspNet.Mvc"
                ]
            }) "core.nuspec"
)

Target "Default" DoNothing

Target "CreatePackages" DoNothing

"Clean"
   ==> "AssemblyInfo"
   ==> "BuildApp"

"BuildApp"
   ==> "CreateCorePackage"
   ==> "CreatePackages"

RunTargetOrDefault "Default"
