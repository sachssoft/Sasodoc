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
* [Feature] Add XML, INI, YAML, and Binary document format support.
* [Feature] Add synchronous and asynchronous stream I/O with cancellation support.
* [Feature] Add Read/Write Version support.
* [Feature] Add AOT-friendly serialization generators based on `ISerializable`.
* [Feature] Add format-independent `DataIdentifier` with support for semantic identifier kinds.
* [Feature] Binary: Add configurable magic values, identifier modes, registries, and length encodings.
* [Change] Frameworks: Drop support for .NET 5, .NET 6, and .NET 7; target .NET 8, .NET 9, and .NET 10.
* [Change] API: Root namespace changed from `Sachssoft.Sasodoc` to `Sachssoft.Documents`.
* [Change] Reader/Writer API: Replace arbitrary `object context` and property identifiers with `DataIdentifier`.
* [Change] Reader/Writer API: Remove `TProperty` and simplify generic format reader/writer types.
* [Change] Reader/Writer API: Remove obsolete and ambiguous overloads with optional context and fallback parameters to provide a more consistent API.
* [Change] Serialization: Move common serialization infrastructure into Sasodoc.
* [Change] API: Clean up and correct the public API by removing unused, redundant, or unintentionally exposed classes, structs, and members.
* [Improve] Add trimming and Native AOT compatibility analysis.
* [Improve] Strengthen build-time code quality checks and treat warnings as errors.
* [Improve] Preserve semantic identifiers independently of naming conventions.
* [Improve] Split document formats into dedicated JSON, XML, INI, YAML, and Binary packages.
* [Improve] Project: Migrate solution format from `.sln` to `.slnx`.
* [Bug] Fix inconsistent Reader/Writer member signatures that could cause ambiguous or error-prone calls, particularly combinations such as `(object context = null)` and `(object context = null, fallback)`.
* [Bug] Fix multiple Reader/Writer conversion and array handling inconsistencies.

## Version 1.0.0 - 2026-04-10
* Initial release.
* Supports .NET 5.0, .NET 6.0, and .NET 7.0.

