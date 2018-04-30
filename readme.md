# Features View Engine

## Description

Features View Engine is an ASP.NET MVC view engine which enables feature folder support. It resolves view paths using the controller's namespace.

## Installation

Install using NuGet:

```powershell
Install-Package FeaturesViewEngine
```

## Configuration

### Default configuration

There is a default view engine - _DefaultControllerFeaturesViewEngine_. Register this view engine on the application start (usually in Global.asax):

```csharp
ViewEngines.Engines.Insert(0, new DefaultControllerFeaturesViewEngine());
```

This view engine will resolve views by the controller's namespace. In these locations:

```
%feature%/{0}.cshtml
%feature%/Views/{0}.cshtml
%feature%/Views/{1}{0}.cshtml
```

The _%feature%_ placeholder will match the controller's namespace excluding an assembly name. For example, you have a controller in the namespace - _MyAssembly.Web.Features.HomePage_ and the assembly name is - _MyAssembly.Web_. Then the _%feature%_ placeholder will be replaced with path - _~/Features/HomePage_.

NOTE: This view engine resolves only paths for views which have a controller. You should create a separate view engine for views which are rendered without a controller.

### Custom paths

If you have a different convention for view path, then you can create a derived view engine from _ControllerFeaturesViewEngine_ and define your paths. You can user _DefaultControllerFeaturesViewEngine_ as a template:

```csharp
public sealed class DefaultControllerFeaturesViewEngine : ControllerFeaturesViewEngine
{
    public DefaultControllerFeaturesViewEngine()
    {
        var paths = new[]
        {
            $"{FeaturePlaceholder}/{{0}}.cshtml",
            $"{FeaturePlaceholder}/Views/{{0}}.cshtml",
            $"{FeaturePlaceholder}/Views/{{1}}{{0}}.cshtml"
        };

        ViewLocationFormats = paths;
        PartialViewLocationFormats = paths;
    }
}
```

_FeaturePlaceholder_ is a constant for _%feature%_.

### Custom assembly name

Sometimes assembly name doesn't match the default namespace name. Then you should create a derived view engine from _ControllerFeaturesViewEngine_ (see "Custom paths" section) and override _NamespacePrefixToRemove_ method to return your namespace prefix to ignore.

### Custom view path format

In some cases, it is useful to have a different view path format. For example, in a multisite environment, you might want to have different view paths for the same feature.

Then you can create a derived view engine (see "Custom paths" section) and override _FormatViewPath_ method with your view path resolution logic.
