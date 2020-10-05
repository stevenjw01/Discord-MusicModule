using System;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Discord.Audio;
using IdiotBOT;
using System.Diagnostics;
using IdiotBOT.Config;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace IdiotBOT.Modules.Public{

    public class youtube : ModuleBase
    {

        IVoiceChannel channel;
        IAudioClient client;

        private Process CreateStream(string url){


            Process currentsong = new Process{

                StartInfo = new ProcessStartInfo{

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
        //END OF MUSIC SHIT
        [Command("playtest", RunMode = RunMode.Async)]
        public async Task playing(string request, string request2 = "", string request3 = "", string request4 = "")
        {

                string json = "";
                string finalrequest = request + request2 + request3 + request4;
                using (WebClient youtubeClient = new WebClient())
                {
                json = youtubeClient.DownloadString("https://www.googleapis.com/youtube/v3/search?part=snippet&q=" + finalrequest + "&type=video&key=AIzaSyD6ObjXWWEs7hSboMwPALipXJ9buzGlPbY");
                }

                

                var dataObject = JsonConvert.DeserializeObject<dynamic>(json);

                string onetitle = dataObject.items[0].snippet.title.ToString();
                string oneid = dataObject.items[0].id.videoId.ToString();
                string oneurl = "https://www.youtube.com/watch?v=" + oneid;

                var embed = new EmbedBuilder();
                embed.WithTitle("🔊 " + "Now Playing" + " 🔊");
                embed.AddField(new EmbedFieldBuilder() { Name = "Song", Value = onetitle });
                embed.AddField(new EmbedFieldBuilder() { Name = "Music Commands", Value = ".play | .stop" });
                embed.WithFooter("Song requested by: " + Context.User.Username);
                embed.WithCurrentTimestamp();
                embed.WithThumbnailUrl("https://yt3.ggpht.com/45DvYS3uqJNb9RUnuZfBEH23YjA5b92p6mrBRS4-aP-Ew0WKXr381qiGQCnRihNn7Bh_eUElZ4XNl81kf3E=s900-mo-c-c0xffffffff-rj-k-no");
                embed.WithColor(new Color(255, 0, 0));
                await Context.Channel.SendMessageAsync("", false, embed);


                IVoiceChannel channel = (Context.User as IVoiceState).VoiceChannel;
                IAudioClient client = await channel.ConnectAsync();

                var output = CreateStream(oneurl).StandardOutput.BaseStream;
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
            await StopAudio(Context.Guild);

            var messages = await Context.Channel.GetMessagesAsync(1).Flatten();
            await Context.Channel.DeleteMessagesAsync(messages);


        }



    }





}