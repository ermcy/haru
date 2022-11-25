using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Flurl.Http;
using Newtonsoft.Json;

namespace StableDiffusion.Commands;

public class Txt2ImgParmameters
{
    public bool enable_hdr = false;
    public double denoising_strength = 0.0;
    public int firstPhase_width = 0;
    public int firstPhase_height = 0;
    public string prompt = "Cirno in a lake with frogs";
    public string? styles;
    public int seed = -1;
    public int subseed = 0;
    public int subseed_resize_from_h = 0;
    public int subseed_resize_from_w = 0;
    public string sampler_name = "";
    public int batch_size = 1;
    public int n_iter = 1;
    public int steps = 25;
    public int cfg_scale = 7;
    public int width = 512;
    public int height = 512;
    public bool restore_faces = false;
    public bool tiling = false;
    public string negative_prompt = "mutilated body";
    public string? eta;
    public int s_churn = 0;
    public int s_tmax = 0;
    public int s_tmin = 0;
    public int s_noise = 0;
    public object override_settings = new { };
    public string sampler_index = "Euler";
}

class DefaultCommands : BaseCommandModule
{

    private static readonly string Url = Environment.GetEnvironmentVariable("API") + "/sdapi/v1/txt2img";

    [Command("ping"), Description("pong")]
    private async Task Ping(CommandContext ctx, [RemainingText] string prompt)
    {
        DiscordEmbedBuilder discordEmbed = new DiscordEmbedBuilder();
        DiscordMessageBuilder messageBuilder = new DiscordMessageBuilder();
        DiscordMessage lastMessage = await ctx.RespondAsync("just a sec");
        IFlurlResponse? FirstResponse = await Url.PostJsonAsync(new Txt2ImgParmameters { steps = 10 , prompt = prompt, width = 256, height = 256  });
        dynamic SecondResponse = await FirstResponse.GetJsonAsync();
        await lastMessage.DeleteAsync();
        discordEmbed.Description = JsonConvert.SerializeObject(SecondResponse.parameters, Formatting.Indented);
        messageBuilder.WithFile(fileName: $"{ctx.Message.Author.Username}.png"
            , stream: new MemoryStream(Convert.FromBase64String(SecondResponse.images[0])));
        messageBuilder.WithEmbed(discordEmbed);
        // messageBuilder.WithReply(ctx.Message.Id, true);
         //Console.WriteLine(JsonConvert.SerializeObject(SecondResponse.parameters));
        await ctx.RespondAsync(messageBuilder);
    }
}