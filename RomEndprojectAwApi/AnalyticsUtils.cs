using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RomEndprojectAwApiRikun.Models;

namespace RomEndprojectAwApiRikun
{
    public class AnalyticsUtils
    {


        //Get the domain Emotion data
        public DomainEmotion GetDomainEmotion(CustomDomainEmotion customDomainEmotion)
        {

            string domain = customDomainEmotion.Domain;
            List<StartEndTableModel> startEndTable = customDomainEmotion.StartEndTable;
            List<Emotion> emotions = customDomainEmotion.Emotions;
            DomainEmotion runningDomainEmotion = new DomainEmotion() { Domain = domain, Happiness = 0, Anger = 0, Disgust = 0, Neutral = 0, Sadness = 0, Surprise = 0 };
            double Anger = 0;
            double Sadness = 0;
            double Neutral = 0;
            double Happiness = 0;
            double Disgust = 0;
            double Surprise = 0;

            foreach (var domainRow in startEndTable.Where(a => a.Domain == domain))
            {
                var anotherDummyVar = emotions
                    .Where(a => a.Time >= domainRow.StartTime && a.Time <= domainRow.EndTime);
                Anger += (double)(anotherDummyVar.Select(a => a.Anger).Sum());
                Sadness += (double)(anotherDummyVar.Select(a => a.Sadness).Sum());
                Neutral += (double)(anotherDummyVar.Select(a => a.Neutral).Sum());
                Happiness += (double)(anotherDummyVar.Select(a => a.Happiness).Sum());
                Disgust += (double)(anotherDummyVar.Select(a => a.Disgust).Sum());
                Surprise += (double)(anotherDummyVar.Select(a => a.Surprise).Sum());

            }
            // Convert to percentage
            double totalSum = (double)(Anger + Disgust + Happiness + Neutral + Sadness + Surprise);

            if (totalSum != 0)
            {
                runningDomainEmotion.Anger = (int)(100 * Anger / totalSum);
                runningDomainEmotion.Sadness = (int)(100 * Sadness / totalSum);
                runningDomainEmotion.Neutral = (int)(100 * Neutral / totalSum);
                runningDomainEmotion.Happiness = (int)(100 * Happiness / totalSum);
                runningDomainEmotion.Disgust = (int)(100 * Disgust / totalSum);//NEEDS TO CHANGE
                runningDomainEmotion.Surprise = (int)(100 * Surprise / totalSum);
            }


            return runningDomainEmotion;
        }
    }
}
