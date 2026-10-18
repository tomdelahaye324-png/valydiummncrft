using CmlLib.Core;
using CmlLib.Core.Auth;
using CmlLib.Core.Auth.Microsoft;
using CmlLib.Core.ProcessBuilder;

namespace Valydium;

public class MainForm : Form
{
    const string McVersion = "1.21.1";
    const string ServerAddress = "join1.severoo.com:55230";
    static readonly Color Bg = Color.FromArgb(14, 10, 22);
    static readonly Color Violet = Color.FromArgb(139, 63, 255);

    readonly Label status = new();
    readonly Button play = new();
    readonly ProgressBar bar = new();

    public MainForm()
    {
        Text = "VALYDIUM Launcher";
        ClientSize = new Size(760, 440);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = Bg;
        ForeColor = Color.White;
        Font = new Font("Segoe UI", 10f);

        Add("VALYDIUM", 40, 40, 680, 70, new Font("Segoe UI", 40f, FontStyle.Bold), Violet);
        Add($"Minecraft Java Edition {McVersion}", 44, 120, 680, 28, new Font("Segoe UI", 13f), Color.Silver);
        Add($"Serveur : {ServerAddress}", 44, 152, 500, 28, new Font("Segoe UI", 11f), Color.Gainsboro);

        var copy = new Button { Text = "Copier l'adresse", Left = 560, Top = 150, Width = 150, Height = 30,
            FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(34, 24, 56), ForeColor = Color.White };
        copy.FlatAppearance.BorderColor = Violet;
        copy.Click += (_, _) => { Clipboard.SetText(ServerAddress); SetStatus("Adresse copiée dans le presse-papiers."); };
        Controls.Add(copy);

        play.Text = "JOUER";
        play.SetBounds(44, 220, 666, 80);
        play.FlatStyle = FlatStyle.Flat;
        play.BackColor = Violet;
        play.ForeColor = Color.White;
        play.Font = new Font("Segoe UI", 24f, FontStyle.Bold);
        play.FlatAppearance.BorderSize = 0;
        play.Click += async (_, _) => await PlayAsync();
        Controls.Add(play);

        bar.SetBounds(44, 330, 666, 14);
        Controls.Add(bar);

        status.SetBounds(44, 355, 666, 60);
        status.ForeColor = Color.Silver;
        status.Text = "Prêt. Clique sur JOUER.";
        Controls.Add(status);
    }

    void Add(string t, int x, int y, int w, int h, Font f, Color c) =>
        Controls.Add(new Label { Text = t, Left = x, Top = y, Width = w, Height = h, Font = f, ForeColor = c, BackColor = Color.Transparent });

    void SetStatus(string s) => status.Text = s;

    async Task PlayAsync()
    {
        play.Enabled = false;
        try
        {
            var gameDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".valydium");
            var launcher = new MinecraftLauncher(new MinecraftPath(gameDir));

            launcher.FileProgressChanged += (_, e) => Invoke(() =>
            {
                if (e.TotalTasks > 0) bar.Value = Math.Min(100, e.ProgressedTasks * 100 / e.TotalTasks);
                SetStatus($"Installation de Minecraft {McVersion} : {e.ProgressedTasks}/{e.TotalTasks}");
            });

            SetStatus("Connexion Microsoft : une fenêtre de connexion officielle va s'ouvrir...");
            var handler = JELoginHandlerBuilder.BuildDefault();
            MSession session = await handler.Authenticate();   // OAuth Microsoft/Xbox/Minecraft officiel

            SetStatus($"Préparation de Minecraft {McVersion}...");
            var option = new MLaunchOption
            {
                Session = session,
                ExtraGameArguments = new[] { MArgument.FromCommandLine($"--quickPlayMultiplayer {ServerAddress}") }
            };
            var process = await launcher.InstallAndBuildProcessAsync(McVersion, option);
            bar.Value = 100;
            SetStatus("Lancement du jeu, connexion directe au serveur...");
            process.Start();
        }
        catch (Exception ex) when (ex.Message.Contains("not own", StringComparison.OrdinalIgnoreCase)
                                   || ex.Message.Contains("minecraft", StringComparison.OrdinalIgnoreCase) && ex.Message.Contains("profile", StringComparison.OrdinalIgnoreCase))
        {
            SetStatus("Ce compte Microsoft ne possède pas Minecraft Java Edition. Utilise un compte qui le possède.");
        }
        catch (System.Net.Http.HttpRequestException)
        {
            SetStatus("Connexion internet impossible. Vérifie ta connexion puis réessaie.");
        }
        catch (Exception ex)
        {
            SetStatus("Erreur : " + ex.Message);
        }
        finally { play.Enabled = true; }
    }
}
