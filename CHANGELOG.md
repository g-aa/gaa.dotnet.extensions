# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/), and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Изменено

## [1.1.4] - 2025-08-17

- Корректировка `.drone.yml`.

## [1.1.0] - 2025-08-17

### Добавлено

- Добавление `.drone.yml`.
- Добавление проекта `Gaa.Extensions.DotNet` в решение.
- Реализация анонимного освобождения ресурсов `AnonymousDisposable` и `AnonymousAsyncDisposable`.
- Реализация распределенной блокировки для приложения запускаемого в единичном экземпляре.

### Изменено

- Обновлена пакетная база.

## [1.0.0] - 2025-02-02

### Добавлено

- Реализация политик авторизации на основе scopes из метаданных.
- Реализация политик авторизации `AllClaimsAuthorizationRequirement` и `AnyClaimsAuthorizationRequirement`.
- Реализация базового функционала проекта.

[Unreleased]: https://github.com/g-aa/gaa.dotnet.extensions/compare/v1.1.4...master
[1.1.4]: https://github.com/g-aa/gaa.dotnet.extensions/compare/v1.1.0...v1.1.4
[1.1.0]: https://github.com/g-aa/gaa.dotnet.extensions/compare/v1.0.0...v1.1.0
[1.0.0]: https://github.com/g-aa/gaa.dotnet.extensions/releases/tag/v1.0.0