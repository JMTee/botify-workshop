using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace Botify_Workshop.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            // calculate something for us to return
            int length = (activity.Text ?? string.Empty).Length;

            // return our reply to the user
            await context.PostAsync($"You sent {activity.Text} which was {length} characters");

            context.Wait(MessageReceivedAsync);
        }


        // helper methods
        /// <summary>
        /// Helper method to give a reply in Hero Card format.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="cardAction"></param>
        /// <param name="subtitle"></param>
        /// <param name="text"></param>
        /// <param name="cardImage"></param>
        /// <returns></returns>
        private static Attachment GetHeroCard(string title, CardAction cardAction, string subtitle = "", string text = "", CardImage cardImage = null)
        {
            var heroCard = new HeroCard
            {
                Title = title,
                Subtitle = subtitle,
                Text = text,
                Images = new List<CardImage>() { cardImage },
                Buttons = new List<CardAction>() { cardAction },
            };

            return heroCard.ToAttachment();
        }
        /// <summary>
        /// Helper method to give a reply in thumbnail format.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="cardAction"></param>
        /// <param name="subtitle"></param>
        /// <param name="text"></param>
        /// <param name="cardImage"></param>
        /// <returns></returns>
        private static Attachment GetThumbnailCard(string title, CardAction cardAction, string subtitle = "", string text = "", CardImage cardImage = null)
        {
            var thumbnailCard = new ThumbnailCard
            {
                Title = title,
                Subtitle = subtitle,
                Text = text,
                Images = new List<CardImage>() { cardImage },
                Buttons = new List<CardAction>() { cardAction },
            };

            return thumbnailCard.ToAttachment();
        }

        /// <summary>
        /// Helper method for linking resources like images
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static string getAbsUrl(string input)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, input);
        }


    }
}