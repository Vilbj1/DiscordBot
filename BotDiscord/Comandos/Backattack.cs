using Discord;
using Discord.Audio;
using Discord.WebSocket;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
using Microsoft.VisualBasic;
using System.Reflection.PortableExecutable;
using NAudio.CoreAudioApi;
using System.Security.Cryptography.X509Certificates;

namespace BotDiscord.Comandos
{
    public class Backattack : ModuleBase<SocketCommandContext>
    {
        private IAudioClient audioClient;
        [Command("back", RunMode = RunMode.Async)]
        public async Task PlayBack()
        {
            IVoiceChannel voiceChannel = (Context.User as IGuildUser)?.VoiceChannel;

            if (voiceChannel == null)
            {
                await Context.Channel.SendMessageAsync("Você precisa estar em um canal de voz.");
                return;
            }

            // Verifica se o bot já está conectado ao canal
            IAudioClient audioClient = GetAudioClient();

            if (audioClient == null || audioClient.ConnectionState != ConnectionState.Connected)
            {
                audioClient = await voiceChannel.ConnectAsync();
            }

            string mp3FilePath = @"G:\Programas\Audacity\Audios\back.mp3";

            // Carrega o arquivo MP3
            var audioBuffer = LoadMp3File(mp3FilePath);

            if (audioBuffer == null)
            {
                await Context.Channel.SendMessageAsync("Erro ao carregar o arquivo MP3.");
                return;
            }

            // Cria um leitor de áudio para o buffer MP3
            var waveStream = new RawSourceWaveStream(new MemoryStream(audioBuffer), new WaveFormat(48000, 16, 2));
            var output = audioClient.CreatePCMStream(AudioApplication.Music);

            // Transmite o áudio para o canal de voz
            await waveStream.CopyToAsync(output);
            await output.FlushAsync();
        }

        private byte[] LoadMp3File(string filePath)
        {
            try
            {
                using (var reader = new Mp3FileReader(filePath))
                using (var writer = new MemoryStream())
                {
                    WaveFileWriter.WriteWavFileToStream(writer, reader);
                    return writer.ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar o arquivo MP3: {ex.Message}");
                return null;
            }
        }

        private IAudioClient GetAudioClient()
        {
            // Verifica se audioClient já está instanciado e conectado
            if (audioClient != null)
            {
                return audioClient;
            }
            return null;
        }
    }
}
