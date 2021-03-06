# Kentico Xperience Content Reference Module
This module provides a service for easy reverse lookup of content references, whether they are through Xperience page relationships, field references, or widget property references.

Additionally, this component is **extensible**. If it doesn't detect a content relationship used in your solution, you can register your own `IReferenceInspector` to fill in the gap.

## Purpose
Creating content relationships is becoming increasingly popular with the advent of headless CMS and hybrid CMS platforms. However, Kentico Xperience has always promoted the use of structured content, decoupled from the design. This typically entails connecting content through page relationships, field references, and widget properties.

## Solution
This module automatically creates a Kentico smart index for quickly looking up content references. It uses multiple `IReferenceInspector` implementations so that it can find content referenced in page type fields, widget properties, and page relationships. The index makes it easy to perform a reverse lookup -- that is to find where any piece of content is used. It also enables finding all the _child_ content used on a page, including all descendants. So, if you need to find all the content items used on a page, it is a fast and simple query.

### Scenarios
We've encountered many scenarios where the ability to look up where content is used is valuable. Here are some examples:

* An author has a library of content items like heroes, testimonials, and CTAs and needs the ability to quickly see where each content item is used.
* A content manager needs to submit several pages for translation, including all the content items referenced by the page. However, without a solution to find all referenced content, it is very difficult to create a translation package with all required pieces of content.

### Example solutions
To solve the scenarios described above, the following solutions are envisioned using this module as the data source:
#### Content Usage Admin Tab
A Kentico admin UI element that displays where a piece of content is used.
![Content usage example](/images/content-usage-example.png)

#### Workflow action: Send page _and dependencies_ for translation
A workflow action to enable sending a page's content dependencies for translation at the same time it is submitted.
![Send for translation example](/images/send-for-translation-example.png)

#### Workflow action: Synchronize page _and dependencies_
A workflow action to enable synchronizing a page and its dependencies.
![Synchronize page example](/images/synchronize-dependencies-example.png)

## Installation
To install, add the NuGet package, [XperienceCommunity.ContentReferenceModule](https://www.nuget.org/packages/XperienceCommunity.ContentReferenceModule), to your CMS project. It will automatically create the smart index required.

## Usage
### Querying content references in a CMS project
After adding the NuGet package to your solution, you can use Kentico's service locator, or constructor-based DI to access the `ContentReferenceService` and call `IContentReferenceService.GetParentReferences(ITreeNode)`.

Here's an example of using the service in the CMS project:
```
var contentReferenceService = Service.Resolve<IContentReferenceService>();
var listOfWhereNodeIsUsed = contentReferenceService
                                .GetParentReferences(treeNode);
```
### Querying content references in a website project
You can also add the NuGet package to your website, if you need to access the `ContentReferenceService` service there. In .NET Core/6+, add the required dependencies to your service collection using the `AddContentReferenceService()` extension method:

```
using XperienceCommunity.ContentReferenceModule.Infrastructure.Extensions;

...

services.AddContentReferenceService();
```

Query content references by injecting `IContentReferenceService`:
```
var listOfWhereNodeIsUsed = _contentReferenceService
                                .GetParentReferences(nodeGuid, "en-us");
```
Or

```
var listOfWhereNodeIsUsed = _contentReferenceService
                                .GetParentReferences(treeNode);

```

## Troubleshooting
This solution uses Xperience smart search. Ensure the index "" is created, built, and propagated to all members of the web farm.
## Compatibility
* .NET 4.8 or higher for the admin app or MVC5 projects
* .NET Core 3.1 or higher for .NET Core/.NET 6 for projects
* Kentico Xperience versions 13.0.0 or higher

## License
This project uses a standard MIT license which can be found here.

## Contribution
Contributions are welcome. Feel free to submit pull requests to the repo.

## Support
Please report bugs as issues in this GitHub repo. We'll respond as soon as possible.

There are existing know issues and plans for this repo here:

https://github.com/heywills/xperience-content-reference-module/issues

