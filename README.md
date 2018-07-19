# cbt_vsts_csharp
Example of setting up tests that can be run from Visual studio team services
<h2>Create a Unit Test Project and add Selenium Webdriver</h2>
Open up Visual Studio 2017. Under Visual C# choose Test and create a new Unit Test Project. After your project is created add Selenium Webdriver to your project by going to Tools -&gt; Nuget Package Manager -&gt; Manage Nuget Packages for Solution. Then select “selenium.webdriver”.
<h2>Copy the Unit Test Code into your Project and publish a repository to VSTS Git</h2>
Copy the example C# code found <a href="https://github.com/crossbrowsertesting/cbt_vsts_csharp/blob/master/cbt_vsts_csharp/UnitTest1.cs">here</a> into Visual Studio. Click on Add to Source Control (located in the bottom right corner of Visual Studio). Choose Git.
When the push panel comes up in Team Explorer click on the publish git repo button within the Visual Studio Team Services box. Choose the visual studio account and domain that you wish to use and name your repository. Click publish repository.
<h2>Create and Queue a Build for Visual Studio Team Services</h2>
Go to your visual studio team services dashboard and select the repository you just created. Click Set up Build.
Choose VSTS Git, the team project, repository, and the branch you want to use and click continue. Choose .Net Desktop and click apply.
Click on Process and set a name for your CI build. Make sure the Agent queue is Hosted VS2017.
Click on Trigger and select Enable continuous integration. Then make sure the Type entry is "include" and the branch is the one that you are using for this build.
Click on Tasks. Click on the plus sign beside Phase 1. Choose Archive Files and select Add. Put $(Build.Repository.LocalPath) for the “Root folder or file to archive” choice. Make sure the Archive task is listed right above the Publish Artifact task by dragging the Archives task into position.
Go to the VsTest (Visual Studio Test) task. Add a line to the Test Assemblies entries like the following “**\$(BuildConfiguration)\*bin\Release\{Insert Name of your Project here}.dll”. For my cbt_vsts_csharp project I enter “**\$(BuildConfiguration)\*bin\Release\cbt_vsts_csharp.dll”.
Select Save and Queue. Select Save and Queue again. Click on the Build Number to be taken to the build run page.
<h2>Create a Release to an Azure App Service</h2>
After the build runs click on Release. Choose Azure App Service deployment and click Apply. Click the lightning bolt in the artifacts box and enable continuous deployment. Add a filter for the master branch. Click in the rectangle in the Artifacts box and change the Source alias to drop. Click on the tasks tab. Choose your Azure subscription. Choose the App service name that goes along with the subscription.
Click on Tasks. Click the plus sign beside run on agent and select Visual Studio Test task. Modify the Test assemblies entry to include **\$(BuildConfiguration)\*bin\Release\{name of your project}.dll. Save the release pipeline. Click on the + Release button. Choose Create Release.
Click Create. Once again you can click on the Release number to be taken to the View Release page. If you click on Logs it will let you watch the output from your release tasks.

Now when you push to Visual Studio Team Services the build and release pipelines will both execute automatically.
