# CONTRIBUTING.md

## Table of Contents
1. [Commit Guidelines](#commit-guidelines)
2. [Branch Naming](#branch-naming)
3. [Pull Requests](#pull-requests)
4. [Additional Guidelines](#additional-guidelines)

## Commit Guidelines
When making commits, use the appropriate gitmoji icon followed by an action (update/add/fix/merge/move) and a short description in English. Here are the icons and their meanings:

- 🔨 `:hammer:` : Makefile/CMake
- 🔝 `:top:` : Header files
- 📚 `:books:` : Lib-related changes
- ✅ `:white_check_mark:` : Add, update or pass tests
- 🧪 `:test_tube:` : Add a failing test
- 📝 `:memo:` : Documentation
- 🙈 `:see_no_evil:` : Gitignore
- 📦 `:package:` : Package.json
- 🚚 `:truck:` : Move or rename resources
- 🔥 `:fire:` : Remove files
- 🔀 `:twisted_rightwards_arrows:` : Merge branch
- 👷 `:construction_worker:` : Add or update automation tools
- 💚 `:green_heart:` : Fix CI build
- 🍌 `:banana:` : Norm
- ✨ `:sparkles:` : New feature of the project
- ✨✨ `:sparkles::sparkles:` : Major new features
- 🐛 `:bug:` : Bug fixes
- 🚑 `:ambulance:` : Critical hotfixes
- 🔧 `:wrench:` : General fix for something that wasn't working as expected
- 🚨 `:rotating_light:` : Fix compiler warnings
- ➕ `:heavy_plus_sign:` : New file, new function
- ➖ `:heavy_minus_sign:` : Deletion of some code
- ♻️ `:recycle:` : Refactor code
- 🚀 `:rocket:` : Final commit

### Example Commit Messages
- 🔨 update Makefile to include new targets
- ✨ add new feature for user authentication
- 🐛 fix bug in login function

## Branch Naming
Branch names should be concise and descriptive, using two or more English words separated by a hyphen (`-`).

### Example Branch Names
- `feature-login`
- `bugfix-authentication`
- `update-readme`

## Pull Requests
### Requirements
- **Review:** At least 2 reviews is required before merging to the main branch.
- **CI:** All GitHub Actions must pass successfully.
### Naming
- **Title:** Use an emoji (gitmoji rule) followed by a short title.
- **Description:** Include bullet points for:
    - What you have done
    - How to check that what you have done is working (if needed)

### Example Pull Request
**Title:** ✨ Add user authentication

**Description:**
- Implemented user login and registration
- Added JWT token generation
- Remaining: Write unit tests for authentication
- To test: Run the application and try logging in with a new user

## Additional Guidelines
- **Documentation:** Update documentation for any new features or changes.
- **Testing:** Ensure that your code is well-tested and that all tests pass before submitting a pull request.

By following these guidelines, you help maintain the quality and consistency of the project. Thank you for your contributions!