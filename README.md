# Sachssoft Sasodoc

**Sasodoc** is a lightweight developer tool that helps simplify serialization between structured data formats and document models (format ⇄ data).

Its goal is to reduce the effort required to process, map, and work with structured data.

---

## ✨ Current Status

Sasodoc is in its **initial release**.

* Current focus: **JSON**
* Additional formats are planned for the long term

---

## ⚠️ Important Note on Serialization

Unlike traditional serializers, Sasodoc currently does **not support full recursive serialization of complete object structures**.

This functionality is **not yet implemented**, but may be introduced in future versions.

Instead, Sasodoc currently uses a **simplified, component-based approach**, allowing individual parts of a data structure to be processed independently.

---

## 🧠 Purpose

Sasodoc is designed to help developers:

* work more easily with structured data
* simplify mapping between data and document structures
* reduce repetitive serialization effort
* improve workflows when dealing with configuration and data files

---

## 📦 NuGet Packages

| Format | Status         | Minimum .NET Version | NuGet |
| ------ | -------------- | -------------------- | ----- |
| Basic   | ✅ Available    | 5.0				 | [![NuGet](https://img.shields.io/nuget/v/Sachssoft.Sasodoc)](https://www.nuget.org/packages/Sachssoft.Sasodoc)      |
| JSON   | ✅ Available    | 7.0				 | [![NuGet](https://img.shields.io/nuget/v/Sachssoft.Sasodoc.Json)](https://www.nuget.org/packages/Sachssoft.Sasodoc.Json)      |
| XML    | 🚧 Coming Soon |						 | |
| YAML   | 🚧 Coming Soon |						 |       |
| TOML   | 🚧 Coming Soon |						 |       |

---

## 🚀 Roadmap

Planned improvements include:

* Support for additional formats (XML, YAML, TOML)
* Enhanced serialization capabilities
* Potential full object processing
* Further simplification of developer workflows

---

## 🤝 Contributing

Contributions, ideas, and feedback are welcome.
