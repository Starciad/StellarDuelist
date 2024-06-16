<div align="center">
	<img width="85%" src=".github/assets/imgs/logo/sd_logo.webp"/>
</div>

<h1 align="center">✨ ● Stellar Duelist ● ✨</h1>

Welcome to the exciting world of **Stellar Duelist!** This is a **Shoot-Em-Up** game where you take control of a spaceship with the mission to exterminate malicious aliens and free space from their evil presence.

With engaging and straightforward gameplay, you will embark on an endless journey, facing dozens of waves of aliens until you reach the universe's major threats, known as "bosses." These challenges will test the skills you acquire throughout the game.

<br/>
<div align="center">
	<img width="100%" src=".github/assets/imgs/borders/b_1.webp"/>
</div>

<div align="center">
	<img width="100%" src=".github/assets/imgs/gifs/a_1.webp"/>
</div>

<div align="center">
	<img width="100%" src=".github/assets/imgs/borders/b_1.webp"/>
</div>
<br/>

## 📚 ➥ Description

Stellar Duelist unfolds in a space setting with infinite and procedural gameplay. Each enemy in the game is carefully selected and randomly positioned at the beginning of each level. Furthermore, players can acquire enhancements that strengthen them as they progress in the game.

Your sole objective is to survive as long as possible, facing threats in space. Get ready for an exciting and challenging journey!

# 🧩​​​ ➥ Download

You can find the latest versions of the game for download on the following platforms:

[![Itch.io](.github/assets/imgs/buttons/b_itch.webp)](https://starciad.itch.io/stardust-defender)
[![GitHub](.github/assets/imgs/buttons/b_github.webp)](https://github.com/Starciad/StellarDuelist)

# 🕹️ ➥​ Requirements

Below you can find an overview of the requirements that are needed to make the game run correctly.

> Building the source code will also require the same requirements.

- **Operating System:** Windows, MacOs, or Linux;
- **DirectX:** Version 9.0c or higher;
- **OpenGL:** Version 2.0 or higher;
- **RAM:** At least 500 MB;
- **Disk Space:** Minimum of 100 MB available;
- **System Architecture:** x64 (64-bit);
- **.NET Runtime:** Required for the game to run.

# 🛠 ➥ Compilation/Building

To compile the project, follow the steps below. Ensure that all prerequisites are met before starting.

## Prerequisites

1. **Install .NET SDK 7.0**: Download and install the [SDK 7.0](https://dotnet.microsoft.com/en-us/download/dotnet/7.0) for .NET on your operating system.
2. **Install Git**: Ensure Git is installed on your environment.

## Cloning the Repository

Open the terminal and clone the repository using the following command:

```bash
git clone https://github.com/Starciad/StellarDuelist.git
```

Once the process is complete, you will have access to all the project's source files.

## Downloading the Assets

To obtain the game assets, follow these steps:

1. Go to the [game page on Itch.io](https://starciad.itch.io/stellar-duelist).
2. Download the file `sd-{version}-assets-full.tar.7z`.
3. Extract the contents of the downloaded file to a directory of your choice.
4. Navigate to the extracted directory and locate the `Content` folder.

## Configuring the Assets

Copy the `Content` directory into the main project directory at the following path:

```txt
{your_project_path}\src\StellarDuelist\
```

## Building the Project

Make sure you are in the directory where the project files (`StellarDuelist.DesktopGL.csproj` and `StellarDuelist.WindowsDX.csproj`) are located:

```txt
{your_project_path}\src\StellarDuelist\
```

To build the project, use the terminal and run the following command:

```bash
dotnet build --project StellarDuelist.{platform}.csproj
```

To build and run the project, use the command:

```bash
dotnet run --project StellarDuelist.{platform}.csproj
```

> **Note**: Replace `{platform}` with the desired platform (`DesktopGL` or `WindowsDX`), according to your operating system or preference.

## Running the Game

If all steps are followed correctly, the game should start without any issues.

# 📜 ➥ Changelog

To find out more about the game's changelog, consult the [Changelog](CHANGELOG.md) file.

# ⁉ ➥ Frequently Asked Questions

Have a question about the project? Check the [F.A.Q.](FAQ.md) to see if it has already been answered!

# 💼 ➥ Contributions

For more information on how to contribute to the project, [click here](CONTRIBUTING.md).

# 🤝 ➥ Code of Conduct

For more information regarding our code of conduct, [click here](CODE_OF_CONDUCT.md).

# 🙌 ➥ Credits

For more details about the credits, [click here](CREDITS.md).

# 📄 ➥ License

This project is under the MIT license. See the [LICENSE](LICENSE.txt) file for more details.

<br/>
<div align="center">
	<img width="100%" src=".github/assets/imgs/borders/b_1.webp"/>
</div>

<div align="center">
	<img width="100%" src=".github/assets/imgs/gifs/a_2.webp"/>
</div>

<div align="center">
	<img width="100%" src=".github/assets/imgs/borders/b_1.webp"/>
</div>
