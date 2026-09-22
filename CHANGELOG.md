# Sachssoft Sasodoc Changelog

All notable changes to this project will be documented in this file.

> This is an early alpha release. Not all planned features are implemented yet. APIs and features may change in future releases.

<small>

## Changelog Legend

| Tag     | Description        | Note                                                                                                                   |
| ------- | ------------------ | ---------------------------------------------------------------------------------------------------------------------- |
| Feature | New feature or API | Adds new functionality                                                                                                 |
| Change  | Functional change  | Changes behavior and may include breaking changes in Alpha and Beta versions                                           |
| Improve | Improvement        | Improves or refactors existing functionality and architecture. May include breaking changes in Alpha and Beta versions |
| Bug     | Bug fix            | Fixes errors or unexpected behavior                                                                                    |

> **Notice:** Breaking changes may occur only in Alpha and Beta versions.

</small>

## Version 2.0.0 - 2026-05-22
* [Feature] Add Read/Write Version support.
* [Feature] Add AOT-friendly serialization generators based on `ISerializable`.
* [Change] API: Root namespace changed from `Sachssoft.Sasodoc` to `Sachssoft.Documents`.
* [Change] Reader/Writer API: Replace arbitrary context/property identifiers with the new `DataIdentifier` abstraction.
* [Change] API: Remove `TProperty` from generic format readers and writers.
* [Improve] Project: Solution format migrated from `.sln` to the modern `.slnx` format.
* [Improve] Preserve special semantic identifiers independently of property and field naming conventions.
* [Improve] Simplify and strengthen the Reader/Writer API with strongly typed identifiers and fewer compatibility bridges.

## Version 1.0.0 - 2026-04-10
* Initial release.
