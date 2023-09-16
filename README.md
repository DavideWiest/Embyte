# Embyte: Your Open-Source Embed Solution

Welcome to Embyte, your go-to open-source embed creator for crafting minimalistic embeds that you can seamlessly use in iFrames. This tool is designed to make web embedding a breeze, drawing inspiration from platforms like Discord and other social media channels.

## Motivation

The inspiration for Embyte stemmed from the desire to simplify web embedding and provide a universal solution. If you've ever admired web-embeds on platforms like Discord and wanted to implement a similar feature, Embyte is here to fulfill that need. Whether you're looking to enhance your website or enhance your personal knowledge management in tools like Obsidian, Embyte offers the versatility you seek.

## Key Features

Embyte offers a range of features to empower your web embedding journey:

### Minimalistic Embeds

Craft sleek and minimalistic embeds that seamlessly blend into your web content, ensuring a visually pleasing user experience.

### Open Source

Embyte is committed to the principles of open source. You have full access to the codebase, allowing for customization and community collaboration.

### iFrame Integration

Easily incorporate your created embeds into iFrames, making them ready for integration into various platforms and applications.

## Getting Started

To get started with Embyte and begin creating your own stylish embeds, check out our documentation at [https://embyte.davidewiest.com/docs](https://embyte.davidewiest.com/docs).

## Contributing

Join us in improving and expanding Embyte by contributing to our open-source project. Visit our GitHub repository at [https://github.com/DavideWiest/embyte](https://github.com/DavideWiest/embyte) to get involved.

Any feedback or bug-reporting is appreciated. Thank you!
- [Open an issue](https://github.com/DavideWiest/Embyte/issues)
- [Email the developer](mailto:davide.wiest2@gmail.com)

#### [Complete a short feedback form](https://forms.gle/zYeA61AgoGPKT4d26)

## License

Embyte is released under the [MIT License](LICENSE.txt).

## Conventions

- `tw...` Attributes refer to usage in or with tailwind-classes
- Reusable components are stored in the `Shared`-folder, Pages in `Pages`
- static content is located in `static`, not `wwwroot`
- Account-related components such as a login-page belong to `identity` (which exists in both frontend-folders)
- Frequently used components are stored in `Shared/base`, seperated into subfolders for each respective group
- Important constants, and "magic"-values are stored in the `Constants` class 
- C# Classes, like a dbManager are located in the modules-folder

