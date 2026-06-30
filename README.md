# LogicBuilder.App.Spa.AutoMapperProfiles

[![CI](https://github.com/BpsLogicBuilder/LogicBuilder.App.Spa.AutoMapperProfiles/actions/workflows/ci.yml/badge.svg)](https://github.com/BpsLogicBuilder/LogicBuilder.App.Spa.AutoMapperProfiles/actions/workflows/ci.yml)
[![CodeQL](https://github.com/BpsLogicBuilder/LogicBuilder.App.Spa.AutoMapperProfiles/actions/workflows/github-code-scanning/codeql/badge.svg)](https://github.com/BpsLogicBuilder/LogicBuilder.App.Spa.AutoMapperProfiles/actions/workflows/github-code-scanning/codeql)
[![codecov](https://codecov.io/gh/BpsLogicBuilder/LogicBuilder.App.Spa.AutoMapperProfiles/graph/badge.svg?token=VUFZST0PWQ)](https://codecov.io/gh/BpsLogicBuilder/LogicBuilder.App.Spa.AutoMapperProfiles)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=BpsLogicBuilder_LogicBuilder.App.Spa.AutoMapperProfiles&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=BpsLogicBuilder_LogicBuilder.App.Spa.AutoMapperProfiles)

A .NET Standard 2.0 library that provides AutoMapper profiles for mapping form parameter objects to their corresponding descriptor objects used in Single Page Applications (SPAs) built with Logic Builder.

## Overview

This library serves as a shared location for AutoMapper mapping configurations that transform form parameters (design-time configuration objects) into serializable descriptors (runtime configuration objects). These mappings are essential for converting Logic Builder form definitions into configurations that can be consumed by SPA applications at runtime.

## Key Features

- **Comprehensive Mapping Coverage**: Provides AutoMapper profiles for all form-related parameter-to-descriptor transformations
- **Base Class Mappings**: Handles polymorphic mappings through inheritance hierarchies
- **Connector Profiles**: Special handling for command button connectors with custom property mapping logic
- **Type-Safe Transformations**: Ensures reliable conversions between parameter and descriptor types

## AutoMapper Profiles

The library includes three main AutoMapper profiles:

### 1. ParameterToDescriptorProfile
Maps over 60 parameter types to their corresponding descriptor types, including:
- Form settings (List, Edit, Detail forms)
- Grid configurations (columns, filters, aggregates)
- Control settings (input fields, dropdowns, multi-select)
- Validation configurations
- Request and domain parameters
- Templates and directives

### 2. ConnectorProfile
Handles specialized mappings for connector parameters:
- Maps `ConnectorParameters` to `CommandButtonDescriptor`
- Maps `CommandButtonParameters` to `CommandButtonDescriptor`
- Applies custom transformation logic for button properties

### 3. BaseClassMappings
Manages polymorphic type mappings:
- Maps `IFormItemSettingParameters` to `FormItemSettingDescriptor` and its derived types
- Maps `IDetailItemParameters` to `DetailItemDescriptor` and its derived types

## Dependencies

- **AutoMapper** (v16.1.1): Core object-to-object mapping library
- **LogicBuilder.App.Spa.Forms.Configuration** (v1.0.1): Descriptor types for runtime configuration
- **LogicBuilder.App.Spa.Forms.Parameters** (v1.0.1): Parameter types for design-time configuration

## Target Framework

- .NET Standard 2.0

## License

MIT License - Copyright © BPS 2026

## Related Projects

- [LogicBuilder](https://github.com/BpsLogicBuilder/LogicBuilder) - Main Logic Builder project
- [LogicBuilder.App.Spa.Forms.Configuration](https://github.com/BpsLogicBuilder/LogicBuilder.App.Spa.Forms.Configuration) - Runtime descriptor types
- [LogicBuilder.App.Spa.Forms.Parameters](https://github.com/BpsLogicBuilder/LogicBuilder.App.Spa.Forms.Parameters) - Design-time parameter types

## NuGet Package

This library is available as a NuGet package: `LogicBuilder.App.Spa.AutoMapperProfiles`

Install via Package Manager Console:
- `Install-Package LogicBuilder.App.Spa.AutoMapperProfiles`

Or via .NET CLI:
- `dotnet add package LogicBuilder.App.Spa.AutoMapperProfiles`