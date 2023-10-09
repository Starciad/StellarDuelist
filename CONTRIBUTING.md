# Contributing to Stardust Defender

Welcome to the Stardust Defender project! We are excited to receive your contributions. However, it is important to follow some guidelines to ensure effective collaboration.

## Branching Base

When creating a Pull Request (PR), make sure that your branch is targeting the latest main branch. This will help avoid conflicts and ensure smooth integration of your changes.

## Versioning

We follow the [Semantic Versioning (SemVer)](https://semver.org/) standard for our stable releases. We recommend focusing PRs on `patch` or `minor` changes. For significant changes (breaking changes), discuss in advance to plan integration into our next major release. If it involves removing public properties or methods, mark them with the [`Obsolete`](https://learn.microsoft.com/en-us/dotnet/api/system.obsoleteattribute?view=net-7.0) attribute on the latest release branch.

## Descriptive Titles

When opening Issues or PRs, use titles that clearly describe the purpose. Keep the title concise; additional details should be included in the description.

## Commit Messages

For commit messages, follow the [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/) standard. Ensure each commit describes the change made. If fixing or resolving an open issue, reference it using `#`.

**Examples of good commit messages:**
- `fix: Fix potential memory leak (#142).`
- `feat: Add new entity (#169).`
- `refact: Refactor code of the 5th entity.`
- `feat: Add new GUI component.`

**Examples of incorrect commit messages:**
- `I'm bad.`
- `Tit and tat.`
- `Fixed.`
- `Oops.`

## Code Style

We use the [Microsoft C# coding conventions](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/inside-a-program/coding-conventions) throughout the repository, with some exceptions.

### Key Preferences

- Prefix `S` in file names in the `StardustDefender.Core` project.
- Use `this` to reference fields and properties.
- Avoid asynchronous code in inappropriate contexts.

### Other Preferences

- Avoid excessive use of `var` to maintain code clarity.
- Mark immutable fields as `readonly`.
- Use `internal` and `public` sparingly, depending on the required scope.
- Utilize object initializers whenever possible.

### Inline `out` Declarations

For `out` declarations, prefer the inline form: `SomeOutMethod(42, out var stringified);`.

### Member Ordering

Maintain the ordering of code members as follows:
1. Properties
2. `const` fields
3. Variable fields
4. Events and Delegates
5. Constructors
6. Methods

## Code Review

All code change commits must compile successfully, as verified by our CI. When you open a Pull Request, GitHub will trigger a build action and create PR artifacts. You can track the progress in the Checks section on the PR overview page.

**Note:** PRs that do not build successfully will not be accepted.

# Conclusion

Thank you for dedicating your time and effort to contribute to Stardust Defender. Your contributions are essential to the success of this project. We hope these guidelines make the collaboration process smoother and result in robust, high-quality code.

If you have any questions or need further assistance, please do not hesitate to reach out to the Stardust Defender team or community. Together, we will continue to create an amazing, star-filled game!

Let's build a bright future for Stardust Defender together. Thank you for being part of this journey!