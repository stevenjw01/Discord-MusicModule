using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Discord.Audio;
using IdiotBOT;
using System.Diagnostics;
using System.Linq;
using IdiotBOT.Config;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using Newtonsoft.Json;



//kimura-dev
namespace IdiotBOT.Modules.Public
{
    public class MusicModule : ModuleBase
    {


        IVoiceChannel channel;
        IAudioClient client;

        private Process CreateStream(string url)
        {
            Process currentsong = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/C youtube-dl.exe -o - {url} | ffmpeg -i pipe:0 -ac 2 -f s16le -ar 48000 pipe:1",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            currentsong.Start();
            return currentsong;
        }

        [Command("play", RunMode = RunMode.Async)]
        public async Task playing([Remainder]string request = "")
        {






            IVoiceChannel voicechannel = (Context.User as IGuildUser).VoiceChannel;
            if (voicechannel == null)
            {
                Console.WriteLine(Context.User + " is not in a voicechannel.");
                await ReplyAsync("You must join a voice channel first.");
            }
            else
            {

                Console.WriteLine("Play Command by " + Context.User, ConsoleColor.Cyan);

                string json = "";

                if (request.Length == 0)
                {
                    Console.WriteLine(Context.User + " not enough args MUSIC");
                    await ReplyAsync("The player is not currently playing anything. Use the following syntax to add a song:" + "\n" + "kk play <url-or-search-terms>");
                    return;
                }
                using (WebClient youtubeClient = new WebClient())
                {
                    json = youtubeClient.DownloadString("https://www.googleapis.com/youtube/v3/search?part=snippet&q=" + request + "&type=video&key=AIzaSyD6ObjXWWEs7hSboMwPALipXJ9buzGlPbY");
                }
                var dataObject = JsonConvert.DeserializeObject<dynamic>(json);

                string onetitle = dataObject.items[0].snippet.title.ToString();
                string oneimage = dataObject.items[0].snippet.thumbnails.high.url.ToString();
                string oneid = dataObject.items[0].id.videoId.ToString();
                string oneurl = "https://www.youtube.com/watch?v=" + oneid;

                string twotitle = dataObject.items[1].snippet.title.ToString();
                string twoimage = dataObject.items[1].snippet.thumbnails.high.url.ToString();
                string twoid = dataObject.items[1].id.videoId.ToString();
                string twourl = "https://www.youtube.com/watch?v=" + twoid;

                string threetitle = dataObject.items[2].snippet.title.ToString();
                string threeimage = dataObject.items[2].snippet.thumbnails.high.url.ToString();
                string threeid = dataObject.items[2].id.videoId.ToString();
                string threeurl = "https://www.youtube.com/watch?v=" + threeid;

                string fourtitle = dataObject.items[3].snippet.title.ToString();
                string fourimage = dataObject.items[3].snippet.thumbnails.high.url.ToString();
                string fourid = dataObject.items[3].id.videoId.ToString();
                string foururl = "https://www.youtube.com/watch?v=" + fourid;

                string fivetitle = dataObject.items[4].snippet.title.ToString();
                string fiveimage = dataObject.items[4].snippet.thumbnails.high.url.ToString();
                string fiveid = dataObject.items[4].id.videoId.ToString();
                string fiveurl = "https://www.youtube.com/watch?v=" + fiveid;



                await ReplyAsync("**Please use this search query as reference for song corrections:** \n**1:** " + onetitle +
                "\n**2:** " + twotitle +
                "\n**3:** " + threetitle +
                "\n**4:** " + fourtitle +
                "\n**5:** " + fivetitle

                );

                await ReplyAsync("**Now playing** `" + onetitle + "` \n**To play other songs, use the search above as reference.**");









                IVoiceChannel channel = (Context.User as IVoiceState).VoiceChannel;
                IAudioClient client = await channel.ConnectAsync();

                var output = CreateStream(oneurl).StandardOutput.BaseStream;
                var stream = client.CreatePCMStream(AudioApplication.Music);
                await output.CopyToAsync(stream);
                await stream.FlushAsync().ConfigureAwait(false);











            }



        }







        [Command("join", RunMode = RunMode.Async)]
        public async Task Joining()
        {

            IVoiceChannel channel = (Context.User as IVoiceState).VoiceChannel;
            IAudioClient client = await channel.ConnectAsync();

            await Context.Message.DeleteAsync();



        }
        [Command("leave", RunMode = RunMode.Async)]
        public async Task Leaving()
        {

            await StopAudio(Context.Guild);

            var messages = await Context.Channel.GetMessagesAsync(1).Flatten();
            await Context.Channel.DeleteMessagesAsync(messages);




        }
        [Command("tati", RunMode = RunMode.Async)]
        public async Task Tatiplaying()
        {

            await Context.Message.DeleteAsync();

            string url = "https://www.youtube.com/watch?v=UC6USOV4ewc";

            await ReplyAsync("🔊 TATI 🔊");
            IVoiceChannel channel = (Context.User as IVoiceState).VoiceChannel;
            IAudioClient client = await channel.ConnectAsync();

            var output = CreateStream(url).StandardOutput.BaseStream;
            var stream = client.CreatePCMStream(AudioApplication.Music, 128 * 1024);
            await output.CopyToAsync(stream);
            await stream.FlushAsync().ConfigureAwait(false);


            await Context.Message.DeleteAsync();




        }
        [Command("asian", RunMode = RunMode.Async)]
        public async Task asian()
        {


            await Context.Message.DeleteAsync();

            string[] urls = "https://www.youtube.com/watch?v=sxPMHtiJh30 https://www.youtube.com/watch?v=4U7_Sfqwdl8 https://www.youtube.com/watch?v=oxxgzQbtKMg https://www.youtube.com/watch?v=GvJsTaGF6Dw".Split();
            Random n = new Random();
            string url = urls[n.Next(0, 2)];

            await ReplyAsync("🔊 OH NOOO! Prepare yourself 🔊");
            IVoiceChannel channel = (Context.User as IVoiceState).VoiceChannel;
            IAudioClient client = await channel.ConnectAsync();

            var output = CreateStream(url).StandardOutput.BaseStream;
            var stream = client.CreatePCMStream(AudioApplication.Music, 128 * 1024);
            await output.CopyToAsync(stream);
            await stream.FlushAsync().ConfigureAwait(false);


            await Context.Message.DeleteAsync();


        }
        [Command("6ix9ine", RunMode = RunMode.Async)]
        [Alias("69")]
        public async Task sixnine()
        {


            await Context.Message.DeleteAsync();

            string[] urls = "https://www.youtube.com/watch?v=LgpRaQSTd38 https://www.youtube.com/watch?v=trF5jkkjL2Q https://www.youtube.com/watch?v=ZWHk9bhwr0M https://www.youtube.com/watch?v=agcoz3hLTVU".Split();
            Random n = new Random();
            string url = urls[n.Next(0, 2)];

            await ReplyAsync("🔊 **6ix9ine** 🔊");
            IVoiceChannel channel = (Context.User as IVoiceState).VoiceChannel;
            IAudioClient client = await channel.ConnectAsync();

            var output = CreateStream(url).StandardOutput.BaseStream;
            var stream = client.CreatePCMStream(AudioApplication.Music, 128 * 1024);
            await output.CopyToAsync(stream);
            await stream.FlushAsync().ConfigureAwait(false);


            await Context.Message.DeleteAsync();


        }
        private async Task StopAudio(IGuild guild)
        {
            channel = (Context.User as IVoiceState).VoiceChannel;
            client = await channel.ConnectAsync();

            await client.StopAsync();

        }

        [Command("stop", RunMode = RunMode.Async)]
        public async Task StopCmd()
        {
            IVoiceChannel voicechannel = (Context.User as IGuildUser).VoiceChannel;
            if (voicechannel == null)
            {
                Console.WriteLine(Context.User + " is not in a voicechannel.");
                await ReplyAsync("You must join a voice channel first.");
                return;
            }


            await StopAudio(Context.Guild);

            

            await ReplyAsync("The music has stopped playing.");


        }


    }
}
