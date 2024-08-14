using CmlLib.Core;
using CmlLib.Core.Auth;
using CmlLib.Core.ProcessBuilder;

System.Net.ServicePointManager.DefaultConnectionLimit = 256;

// initialize the launcher
var path = new MinecraftPath();
var launcher = new MinecraftLauncher(path);
Console.WriteLine("Добро пожаловать в Minecraft лаунчер!");
Console.Write("Введите ваш никнейм: ");
var nickname = Console.ReadLine();

Console.WriteLine("Введите версию Minecraft: ");
var version = Console.ReadLine();

// add event handlers
launcher.FileProgressChanged += (sender, args) =>
{
    Console.WriteLine($"Total: {args.TotalTasks}");
    Console.WriteLine($"Progressed: {args.ProgressedTasks}");
};
launcher.ByteProgressChanged += (sender, args) =>
{
    Console.WriteLine($"{args.ProgressedBytes} bytes / {args.TotalBytes} bytes");
};

// get all versions
var versions = await launcher.GetAllVersionsAsync();
bool versionExists = false;

foreach (var v in versions)
{
    Console.WriteLine(v.Name);
    if (v.Name == version)
    {
        versionExists = true;
    }
}

// Check if the version exists
if (!versionExists)
{
    Console.WriteLine("Указанная версия Minecraft не найдена.");
    return;
}

// install and launch the game
await launcher.InstallAsync(version);
var process = await launcher.BuildProcessAsync(version, new MLaunchOption
{
    Session = MSession.CreateOfflineSession(nickname),
    MaximumRamMb = 4096
});
process.Start();
