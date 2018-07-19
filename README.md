# cbt_vsts_csharp
Example of setting up tests that can be run from Visual studio team services
<h2>Create a Unit Test Project and add Selenium Webdriver</h2>
<ol>
  <li>Open up Visual Studio 2017.</li><li> Under Visual C# choose Test and create a new Unit Test Project.</li><li> After your project is created add Selenium Webdriver to your project by going to Tools -&gt; Nuget Package Manager -&gt; Manage Nuget Packages for Solution. </li> <li>Select “selenium.webdriver”.</li>
  </ol>
<h2>Copy the Unit Test Code into your Project and publish a repository to VSTS Git</h2>
<ol><li>Copy the example C# code found <a href="https://github.com/crossbrowsertesting/cbt_vsts_csharp/blob/master/cbt_vsts_csharp/UnitTest1.cs">here</a> into Visual Studio.</li><li> Click on Add to Source Control (located in the bottom right corner of Visual Studio).</li><li> Choose Git.</li><li>
When the push panel comes up in Team Explorer click on the publish git repo button within the Visual Studio Team Services box.</li><li> Choose the visual studio account and domain that you wish to use and name your repository.</li><li> Click publish repository.</li></ol>
<h2>Create and Queue a Build for Visual Studio Team Services</h2>
<ol>
  <li>
    Go to your visual studio team services dashboard and select the repository you just created. </li>
  <li>Click Set up Build.</li>
<li>Choose VSTS Git, the team project, repository, and the branch you want to use and click continue.</li><li> Choose .Net Desktop and click apply.</li>
<li>Click on Process and set a name for your CI build. Make sure the Agent queue is Hosted VS2017.</li>
<li>Click on Trigger and select Enable continuous integration. Then make sure the Type entry is "include" and the branch is the one that you are using for this build.</li>
  <li>Click on Tasks.</li>
  <li>Click on the plus sign beside Phase 1. </li>
  <li>Choose Archive Files and select Add.</li>
  <li>Put $(Build.Repository.LocalPath) for the “Root folder or file to archive” choice. 
  </li><li>Make sure the Archive task is listed right above the Publish Artifact task by dragging the Archives task into position.</li>
  <li>Go to the VsTest (Visual Studio Test) task. </li>
  <li>Add a line to the Test Assemblies entries like the following “**\$(BuildConfiguration)\*bin\Release\{Insert Name of your Project here}.dll”. For my cbt_vsts_csharp project I enter “**\$(BuildConfiguration)\*bin\Release\cbt_vsts_csharp.dll”.</li>
  <li>
    Select Save and Queue. </li><li>Select Save and Queue again. </li><li>Click on the Build Number to be taken to the build run page.</li>
  </ol>
<h2>Create a Release to an Azure App Service</h2>
<ol><li>
  After the build runs click on Release.</li><li> Choose Azure App Service deployment and click Apply.</li><li> Click the lightning bolt in the artifacts box and enable continuous deployment.</li><li> Add a filter for the master branch.</li><li> Click in the rectangle in the Artifacts box and change the Source alias to drop.</li><li> Click on the tasks tab. </li><li>Choose your Azure subscription. </li><li>Choose the App service name that goes along with the subscription.</li>
  <li>Click on Tasks. </li><li>Click the plus sign beside run on agent and select Visual Studio Test task.</li><li> Modify the Test assemblies entry to include **\$(BuildConfiguration)\*bin\Release\{name of your project}.dll. </li><li>Save the release pipeline. </li><li>Click on the + Release button. </li><li>Choose Create Release.</li>
  <li>Click Create.</li>
  <li>Once again you can click on the Release number to be taken to the View Release page. If you click on Logs it will let you watch the output from your release tasks.</li>
  </ol>

Now when you push to Visual Studio Team Services the build and release pipelines will both execute automatically.
