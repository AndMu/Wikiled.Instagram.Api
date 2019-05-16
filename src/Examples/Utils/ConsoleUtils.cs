using System;
using Wikiled.Instagram.Api.Classes.Models.Media;

namespace Examples.Utils
{
    public static class InstaConsoleUtils
    {
        public static void PrintMedia(string header, InstaMedia media, int maxDescriptionLength)
        {
            Console.WriteLine(
                $"{header} [{media.User.UserName}]: {media.Caption?.Text.Truncate(maxDescriptionLength)}, {media.Code}, likes: {media.LikesCount}, multipost: {media.IsMultiPost}");
        }
    }
}