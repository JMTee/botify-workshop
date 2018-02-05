using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.IO;

namespace botify_workshop_done.Dialogs
{

    [Serializable]
    public class RootDialog : IDialog<object>
    {
        private static readonly HttpClient client = new HttpClient();

        public Task StartAsync(IDialogContext context)
        {
            
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            //check if text is null
            if (activity != null && activity.Text != null)
            {
                //send message to user
                await context.PostAsync("Hello I'm the Pizza Hut bot, how may I help you?");
                //proceed to next conversation
              context.Wait(SelectPizzas);
            }
            else
            {
                context.Wait(MessageReceivedAsync);
            }


        }
        private async Task SelectPizzas(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            var reply = context.MakeMessage();
            //check if text is null
            if (activity != null && activity.Text != null)
            {
                //send message to user
                await context.PostAsync("Here are the results");
                reply.Attachments = new List<Attachment>()
                {
                        GetHeroCard(
                    "Combo Pizza",
                    new CardAction(ActionTypes.ImBack, "ORDER this food", value: "Combo Pizza"), "599 PHP", "Family size pepperoni + greens",
                    new CardImage(url: getAbsUrl(@"Resources\combo.png"))
                    ),

                GetHeroCard(
                        "Pepperoni Pizza",
                        new CardAction(ActionTypes.ImBack, "ORDER this food", value: "Pepperoni Pizza"), "500 PHP", "Pepperoni only",
                        new CardImage(url: getAbsUrl(@"Resources\pepperoni.png"))
                        ),

                GetHeroCard(
                "Spagetti",
                new CardAction(ActionTypes.ImBack, "ORDER this food", value: "Spagetti"), "500 PHP", "Tomato Sauce with Meatballs",
                new CardImage(url: getAbsUrl(@"Resources\combo.png"))
                )
               };

                reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                await context.PostAsync(reply);


                //proceed to next conversation
                  context.Wait(getQuantity);
            }
        }

        private async Task getQuantity(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            var reply = context.MakeMessage();
            //check if text is null
            if (activity != null && activity.Text != null)
            {
                //send message to user
                await context.PostAsync("How many "+activity.Text+" would you like?");
              

                reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                context.Wait(GetAddress);
            }
        }

        private async Task GetAddress(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            var reply = context.MakeMessage();
            //check if text is null
            if (activity != null && activity.Text != null)
            {
                //send message to user
                
                await context.PostAsync("That's great! I would just need your current address.");
              
                context.Wait(EndMessage);
            }
        }

        private async Task EndMessage(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            var reply = context.MakeMessage();
            //check if text is null
            if (activity != null && activity.Text != null)
            {
                //send message to user
                await context.PostAsync("Alright! Please expect your pizza to be delivered to you after 30 minutes!");
                await context.PostAsync("Have a great day!");

              
            }
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