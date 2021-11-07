# Sitecore PartDesign Module

![PartDesign Banner](documentation/images/PartDesign-Banner.jpg?raw=true)

Sitecore **PartDesign** provides to any **Vanilla Siteocore XM/XP**, with the same functionalities of [SXA's Partial Design](https://doc.sitecore.com/en/developers/sxa/101/sitecore-experience-accelerator/partial-designs.html): 

 1. Create complex and reusable design elements quickly
 2. Visual content edition experience with Experince Editor
 3. Compatible with [Content Testing](https://doc.sitecore.com/en/developers/sxa/101/sitecore-experience-accelerator/running-a-content-test-on-a-partial-design.html) and Personalization

You can create and update a PartDesign in a very similar way as it's [done with SXA](https://doc.sitecore.com/en/developers/sxa/17/sitecore-experience-accelerator/create-and-change-a-partial-design.html), 

### Author
<img src="documentation/images/Eu%20-%20Mini.jpeg" alt="Rodrigo Peplau" width="170" align="left">

**Rodrigo Peplau** is on:

[Twitter](https://twitter.com/SitecoreSinger) <br/>
[LinkedIn](https://www.linkedin.com/in/rodrigopeplau/) <br/>
Facebook <br/>
Slack <br/>

and other channels...

## How it works

Very similar to the original Partial Design, you can compile multiple components in a single block, which can be later added to any other page. For instance, you can compile your headers and footers into a PartDesign, and then re-use it in all your pages. 

The image below shows one example, where the a composed component called "Promo Card" has three inner components, respective to each of the 3 columns:

![PartDesign in XP Editor](documentation/images/PartDesign%20in%20XP%20Editor.jpg?raw=true)

The same PartDesign can later be easily added to multiple pages:

<img src="documentation/images/PartDesign-being-reused.jpg?raw=true" alt="PartDesign being reused" style="border: 1px solid black;">


## Installation

1. Download the packages from here: [Content](sc.packages/PartDesign%20-%20Content-1.0.zip?raw=true) and [Files](sc.packages/PartDesign%20-%20Files-1.0.zip?raw=true)
2. Using the Installation Wizard, install the **Content package** on your CM node and publish
3. Install the **Files package** on all your CM and CD nodes. 

## Usage

### Creating a PartDesign:
1. Create your part design using the template **/sitecore/templates/Feature/PartDesign/PartDesign**
2. Add your component(s) to the placeholder **part-design** - nested placeholders are accepted
![PartDesign in Layout Details](documentation/images/PartDesign-LayoutDetails.jpg?raw=true)
3. Save the PartDesign normally

### Adding a PartDesign to a page:
1. Add the **/sitecore/layout/Renderings/Feature/PartDesign/Part Design** rendering to any of your placeholders
![PartDesign Rendering added to a page](documentation/images/PartDesign-Rendering.jpg?raw=true)

2. Select the PartDesign created in the previous section as the Datasource of the PartDesign you just added
3. Save the page

### Editing the PartDesign:
While editing a page that has a PartDesign in Experience Editor, components injected by the PartDesign are not directly editable. Instead, you must edit the PartDesign itself, which can be conviniently made by clicking the link "Edit PartDesign"

![PartDesign in XP](documentation/images/PartDesign-edit-XP.jpg?raw=true)


## Configuration (Optional)

To make the module usage more convenient, you can apply the following customizations:

### A) Create a folder to store all your PartDesigns

Create a folder using the template **/sitecore/templates/Feature/PartDesign/PartDesign Folder**. This template already comes with Insert Options configured to create PartDesign Datasource Items, or more folders.

### B) Add your PartDesign folder to the Rendering

To make the selection of PartDesigns more user-friendly, add the folder path created in the previous step to the field **Datasource Location** of the rendering **/sitecore/layout/Renderings/Feature/PartDesign/Part Design**

![PartDesign Rendering - Datasource Location](documentation/images/PartDesign-Rendering_Update.jpg?raw=true)

### C) Add the PartDesign Rendering to your Placeholder(s) Settings

To make the visual selection of the PartDesign rendering more convenient, add it to your Placeholder(s) Setting:

![PartDesign added to placeholder](documentation/images/PartDesign-added-to-placeholder.jpg?raw=true)

### D) Add your components to the PartDesign Placeholder Settings

To make more convenient the addition of custom inside your PartDesign, add your components to the PartDesign Placeholder Settings, located under **/sitecore/layout/Placeholder Settings/Feature/PartDesign/part-design**:

![PartDesign added to placeholder](documentation/images/PartDesign-placeholder-settings.jpg?raw=true)

### E) Add your CSS and JS files to the PartDesign Layout

You can add your CSS and JS files to the PartDesign Layout, by modifying the file located under **\Views\PartDesign\PartDesignLayout.cshtml**. This will make the visual edition more natural and user-friendly, as it will look and behave exactly as in the original page.

![PartDesign layout modifications](documentation/images/PartDesign-Layout.jpg?raw=true)
