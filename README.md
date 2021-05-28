# Kiwix2RACHEL
A Windows utility to build RACHEL modules for use with Kiwix 3.1.2. 

## Interface

![Kiwix2RACHEL](https://user-images.githubusercontent.com/47008209/120028572-b3b08380-bfa9-11eb-9ba2-aba2983e0292.png)

## Instructions

1. Set a title for your module in the "Module Title" field. This will be the clickable link within RACHEL to get to the main page
2. Select the language of the module's content from the language dropdown menu. This will automatically set the language code for the module 
3. Set a folder name for your module. For example "wikipedia". If you had previously selected Spanish for the language, the module name will be es-wikipedia.
4. Set a module version. We currently use the format YEAR.VERSION. For example, 2021.01. If the module is being updated from 2021.01, it would be 2021.02
5. Click the + button to select a zim file. If you don't yet have a zim file, create an empty text file and set it's extension as .zim
6. Click the next + button to add a .png logo file.
7. Enter a module description for the main paragraph as the module
8. For a basic preview of your module ( not 100% accurate layout yet ), click the preview button 
9. Click save and select a location to save your module. When prompted you can choose to copy the zim file to the new module directory.

Your module is now built. To test your module in RACHEL do the following.

1. Install 7zip from https://www.7-zip.org/download.html
2. Right click the new module folder and go to 7-Zip-> Add to Archive. 
3. Make sure the format is .zip, then save the zip with the same name as your module folder
4. Upload the zip through the new settings.php module upload utility


