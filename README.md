# 🛠️ DevTools

**DevTools** is a modern WPF Dashboard application built with .NET that provides a dynamic interface for loading and executing custom developer tools at runtime. Whether you're automating repetitive tasks or exposing useful debug utilities, this dashboard makes it easy to plug in and go.

## ✨ Features

- Dynamically loads assemblies at runtime
- Detects and displays tools that inherit from a shared `DevTool` base class
- Lets users run tasks, view logs, and interact with custom tool UIs
- Designed for extensibility and developer productivity

## 🚀 Quick Start

1. Clone this repo
2. Open the solution in Visual Studio
3. Build and run the `DevToolsDashboard` project
4. Add your own tools under the `DevTools.Tools` namespace

Want to create your first tool?  
👉 [Check out the Wiki](https://github.com/nickmoon0/DevTools/wiki) for step-by-step instructions!


## 🔧 Requirements

- .NET 8 or .NET 9 SDK is required  
  [Download .NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) | [Download .NET 9](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- Any .NET IDE with WPF support  
  (e.g., **Visual Studio 2022+**, **JetBrains Rider**, or similar)

## 📚 Learn More

Check out the [Wiki](https://github.com/nickmoon0/DevTools/wiki) to learn how to:

- Create your own tool
- Add configurable parameters
- Monitor and log tool data
- Run custom tasks in the dashboard