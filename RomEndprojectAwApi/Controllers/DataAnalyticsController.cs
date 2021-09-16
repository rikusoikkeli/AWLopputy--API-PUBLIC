using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RomEndprojectAwApiRikun.Models;
using Microsoft.EntityFrameworkCore;

namespace RomEndprojectAwApiRikun.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataAnalyticsController : ControllerBase
    {

        private readonly romdatabaseawContext _dbContext;

        public DataAnalyticsController(romdatabaseawContext context)
        {
            _dbContext = context;
        }

        [HttpGet]
        public TimeEmotions AllData(string device)
        {
            // Return the joined table with session start and end
            var startEndTable = _dbContext
                .BrowsingStarts
                .Where(a => a.DeviceId == device)
                .Join(_dbContext.BrowsingEnds,
                BS => BS.SessionId,
                BE => BE.SessionId,
                (BS, BE) => new
                {
                    SessionId = BS.SessionId,
                    StartTime = BS.Time,
                    EndTime = BE.Time,
                    Domain = BS.Domain,
                    DeviceId = BS.DeviceId
                }
                )
                .Where(a => a.StartTime != null)
                .Where(a => a.EndTime != null);

            // Calculate total time using TimeSpan:
            TimeSpan totalTimespan = TimeSpan.FromSeconds(0);
            foreach (var item in startEndTable)
            {
                totalTimespan += ((DateTime)item.EndTime - (DateTime)item.StartTime);
            }
            Console.WriteLine("Total time: " + totalTimespan.TotalSeconds);

            // Now connect with the emotion data -> All time stats on emotions
            var timeEmotionData = startEndTable
                .Join(_dbContext.Emotions,
                SE => SE.DeviceId,
                EM => EM.DeviceId,
                (SE, EM) => new
                {
                    DeviceId = SE.DeviceId,
                    Domain = SE.Domain,
                    StartTime = SE.StartTime,
                    EndTime = SE.EndTime,
                    SessionId = SE.SessionId,
                    Happiness = EM.Happiness,
                    Sadnsess = EM.Sadness,
                    Anger = EM.Anger,
                    Neutral = EM.Neutral,
                    Surprise = EM.Surprise,
                    Disgust = EM.Disgust
                }
                )
                .GroupBy(a => a.DeviceId)
                .Select(b => new
                {
                    deviceID = b.Key,
                    HappinessSum = b.Sum(c => c.Happiness),
                    AngerSum = b.Sum(c => c.Anger),
                    SadnessSum = b.Sum(c => c.Sadnsess),
                    SurpriseSum = b.Sum(c => c.Surprise),
                    DisgustSum = b.Sum(c => c.Disgust),
                    NeutralSum = b.Sum(c => c.Neutral),
                    Count = b.Count()
                })
                .FirstOrDefault();

            // Total sum of emotions
            double totalEmotions = (double)(timeEmotionData.HappinessSum 
                + timeEmotionData.SadnessSum 
                + timeEmotionData.AngerSum 
                + timeEmotionData.NeutralSum 
                + timeEmotionData.SurpriseSum 
                + timeEmotionData.DisgustSum);

            // To get the % just devide emotions by the count DOES NOT WORK WITH NULL OR 0
            TimeEmotions result = new TimeEmotions(
                totalTimespan.TotalMinutes, 
                (int)(100 * timeEmotionData.HappinessSum / totalEmotions), 
                (int)(100 * timeEmotionData.SadnessSum / totalEmotions), 
                (int)(100 * timeEmotionData.AngerSum / totalEmotions), 
                (int)(100 * timeEmotionData.NeutralSum / totalEmotions), 
                (int)(100 * timeEmotionData.SurpriseSum / totalEmotions), 
                (int)(100 * timeEmotionData.DisgustSum / totalEmotions));
            return result;
        }

        [HttpGet("7days")]
        public TimeEmotions Data7Days(string device)
        {
            // Return the joined table with session start and end
            var startEndTable = _dbContext
                .BrowsingStarts
                .Where(a => a.DeviceId == device)
                .Join(_dbContext.BrowsingEnds,
                BS => BS.SessionId,
                BE => BE.SessionId,
                (BS, BE) => new
                {
                    SessionId = BS.SessionId,
                    StartTime = BS.Time,
                    EndTime = BE.Time,
                    Domain = BS.Domain,
                    DeviceId = BS.DeviceId
                }
                )
                .Where(a => a.StartTime != null)
                .Where(a => a.EndTime != null)
                .Where(a => (DateTime)a.StartTime >= DateTime.Now.AddDays(-7));

            // Calculate total time with TimeSpan:
            TimeSpan totalTimespan = TimeSpan.FromSeconds(0);
            foreach (var item in startEndTable)
            {
                totalTimespan += ((DateTime)item.EndTime - (DateTime)item.StartTime);
            }
            Console.WriteLine("Total time: " + totalTimespan.TotalSeconds);

            // Now connect with the emotion data -> All time stats on emotions
            var timeEmotionData = startEndTable
                .Join(_dbContext.Emotions,
                SE => SE.DeviceId,
                EM => EM.DeviceId,
                (SE, EM) => new
                {
                    DeviceId = SE.DeviceId,
                    Domain = SE.Domain,
                    StartTime = SE.StartTime,
                    EndTime = SE.EndTime,
                    SessionId = SE.SessionId,
                    Happiness = EM.Happiness,
                    Sadness = EM.Sadness,
                    Anger = EM.Anger,
                    Neutral = EM.Neutral,
                    Surprise = EM.Surprise,
                    Disgust = EM.Disgust
                }
                )
                .GroupBy(a => a.DeviceId)
                .Select(b => new
                {
                    deviceID = b.Key,
                    HappinessSum = b.Sum(c => c.Happiness),
                    AngerSum = b.Sum(c => c.Anger),
                    SadnessSum = b.Sum(c => c.Sadness),
                    SurpriseSum = b.Sum(c => c.Surprise),
                    DisgustSum = b.Sum(c => c.Disgust),
                    NeutralSum = b.Sum(c => c.Neutral),
                    Count = b.Count()
                })
                .FirstOrDefault();
            // Total sum of emotions
            double totalEmotions = (double)(timeEmotionData.HappinessSum 
                + timeEmotionData.SadnessSum 
                + timeEmotionData.AngerSum 
                + timeEmotionData.NeutralSum 
                + timeEmotionData.SurpriseSum 
                + timeEmotionData.DisgustSum);

            // To get the % just devide emotions by the count
            TimeEmotions result = new TimeEmotions(
                totalTimespan.TotalMinutes, 
                (int)(100 * timeEmotionData.HappinessSum / totalEmotions), 
                (int)(100 * timeEmotionData.SadnessSum / totalEmotions), 
                (int)(100 * timeEmotionData.AngerSum / totalEmotions), 
                (int)(100 * timeEmotionData.NeutralSum / totalEmotions), 
                (int)(100 * timeEmotionData.SurpriseSum / totalEmotions), 
                (int)(100 * timeEmotionData.DisgustSum / totalEmotions));
            return result;
        }
        [HttpGet("1Day")]
        public TimeEmotions Data1Day(string device)
        {
            // Return the joined table with session start and end
            var startEndTable = _dbContext
                .BrowsingStarts
                .Where(a => a.DeviceId == device)
                .Join(_dbContext.BrowsingEnds,
                BS => BS.SessionId,
                BE => BE.SessionId,
                (BS, BE) => new
                {
                    SessionId = BS.SessionId,
                    StartTime = BS.Time,
                    EndTime = BE.Time,
                    Domain = BS.Domain,
                    DeviceId = BS.DeviceId
                }
                )
                .Where(a => a.StartTime != null)
                .Where(a => a.EndTime != null)
                .Where(a => (DateTime)a.StartTime >= DateTime.Now.AddDays(-1));

            // Calculate total time using TimeSpan class:
            TimeSpan totalTimespan = TimeSpan.FromSeconds(0);
            foreach (var item in startEndTable)
            {
                if(item.EndTime != null)
                    totalTimespan += ((DateTime)item.EndTime - (DateTime)item.StartTime);
            }
            Console.WriteLine("Total time: " + totalTimespan.TotalSeconds);

            // Now connect with the emotion data -> All time stats on emotions
            var timeEmotionData = startEndTable
                .Join(_dbContext.Emotions,
                SE => SE.DeviceId,
                EM => EM.DeviceId,
                (SE, EM) => new
                {
                    DeviceId = SE.DeviceId,
                    Domain = SE.Domain,
                    StartTime = SE.StartTime,
                    EndTime = SE.EndTime,
                    SessionId = SE.SessionId,
                    Happiness = EM.Happiness,
                    Sadness = EM.Sadness,
                    Anger = EM.Anger,
                    Neutral = EM.Neutral,
                    Surprise = EM.Surprise,
                    Disgust = EM.Disgust
                }
                )
                .GroupBy(a => a.DeviceId)
                .Select(b => new
                {
                    deviceID = b.Key,
                    HappinessSum = b.Sum(c => c.Happiness),
                    AngerSum = b.Sum(c => c.Anger),
                    SadnessSum = b.Sum(c => c.Sadness),
                    SurpriseSum = b.Sum(c => c.Surprise),
                    DisgustSum = b.Sum(c => c.Disgust),
                    NeutralSum = b.Sum(c => c.Neutral),
                    Count = b.Count()
                })
                .FirstOrDefault();

            // Total sum of emotions
            double totalEmotions = (double)(timeEmotionData.HappinessSum 
                + timeEmotionData.SadnessSum 
                + timeEmotionData.AngerSum 
                + timeEmotionData.NeutralSum 
                + timeEmotionData.SurpriseSum 
                + timeEmotionData.DisgustSum);

            // To get the % just devide emotions by the count
            TimeEmotions result = new TimeEmotions(
                totalTimespan.TotalMinutes, 
                (int)(100 * timeEmotionData.HappinessSum / totalEmotions), 
                (int)(100 * timeEmotionData.SadnessSum / totalEmotions), 
                (int)(100 * timeEmotionData.AngerSum / totalEmotions), 
                (int)(100 * timeEmotionData.NeutralSum / totalEmotions), 
                (int)(100 * timeEmotionData.SurpriseSum / totalEmotions), 
                (int)(100 * timeEmotionData.DisgustSum / totalEmotions));
            return result;
        }

        [HttpGet("DomainEmotion")]
        public List<DomainEmotion> DomainEmotionData(string device)
        {
            // Return the joined table with session start and end
            var startEndTable = _dbContext
                .BrowsingStarts
                .Where(a => a.DeviceId == device)
                .Join(_dbContext.BrowsingEnds,
                BS => BS.SessionId,
                BE => BE.SessionId,
                (BS, BE) => new
                {
                    SessionId = BS.SessionId,
                    StartTime = BS.Time,
                    EndTime = BE.Time,
                    Domain = BS.Domain,
                    DeviceId = BS.DeviceId
                }
                )
                .Where(a => a.StartTime != null)
                .Where(a => a.EndTime != null)
                .ToList();


            List<StartEndTableModel> startEndTableModels = new List<StartEndTableModel>();

            var domains = startEndTable.Select(a => a.Domain).Distinct(); //Distinct domains
            foreach (var startEnd in startEndTable)
            {
                startEndTableModels.Add(new StartEndTableModel() { Domain = startEnd.Domain, DeviceId = startEnd.DeviceId, EndTime = (DateTime)startEnd.EndTime, SessionId = startEnd.SessionId, StartTime = (DateTime)startEnd.StartTime });
            }
            List<DomainEmotion> domainEmotionList = new List<DomainEmotion>();
            List<Emotion> emotions = _dbContext.Emotions.ToList();
            AnalyticsUtils analyticsUtils = new AnalyticsUtils();

            foreach (var domain in domains)
            {
                CustomDomainEmotion customDomainEmotion = new CustomDomainEmotion() { Domain = domain, Emotions = emotions, StartEndTable = startEndTableModels };
                domainEmotionList.Add(analyticsUtils.GetDomainEmotion(customDomainEmotion));
            }

            return domainEmotionList;
        }

        [HttpGet("SessionLength")]
        public void GetSessionLength(string device)
        {
        }

        [HttpGet("HourOfDay")]
        public List<HourlyEmotions> GetHourOfDay(string device)
        {
            // Get emotion data for the device
            var emotionData = _dbContext
                .Emotions
                .Where(a => a.DeviceId == device);
            // Create the list where emotiondata is stored per hour 
            List<HourlyEmotions> hourlyEmotion = new List<HourlyEmotions>();
            double totalSum = 0;
            for (int i = 0; i < 24; i++)
            {
                var dummyVar = emotionData
                .Where(a => ((DateTime)a.Time).Hour == i)
                .GroupBy(a => a.DeviceId)
                .Select(b => new
                {
                    HappinessSum = b.Sum(c => c.Happiness),
                    AngerSum = b.Sum(c => c.Anger),
                    SadnessSum = b.Sum(c => c.Sadness),
                    SurpriseSum = b.Sum(c => c.Surprise),
                    DisgustSum = b.Sum(c => c.Disgust),
                    NeutralSum = b.Sum(c => c.Neutral),
                    Count = b.Count()
                })
                .FirstOrDefault();

                if (dummyVar != null)
                {
                    totalSum = (double)(dummyVar.AngerSum 
                        + dummyVar.DisgustSum 
                        + dummyVar.HappinessSum 
                        + dummyVar.NeutralSum 
                        + dummyVar.SadnessSum 
                        + dummyVar.SurpriseSum);

                    hourlyEmotion.Add(new HourlyEmotions {
                        HourOrWeekdayNumber=i, 
                        Anger = (int)(dummyVar.AngerSum*100/totalSum), 
                        Disgust = (int)(dummyVar.DisgustSum*100 / totalSum), 
                        Happiness = (int)(dummyVar.HappinessSum * 100 / totalSum), 
                        Neutral = (int)(dummyVar.NeutralSum * 100 / totalSum), 
                        Sadness = (int)(dummyVar.SadnessSum * 100 / totalSum), 
                        Surprise = (int)(dummyVar.SurpriseSum * 100 / totalSum)
                    });
                }
                else
                {
                    hourlyEmotion.Add(new HourlyEmotions { HourOrWeekdayNumber = i });
                }
            }

            return hourlyEmotion;
        }

        [HttpGet("Weekday")]
        public List<HourlyEmotions> GetWeekday(string device)
        {
            // Get emotion data for the device
            var emotionData = _dbContext
                .Emotions
                .Where(a => a.DeviceId == device && a.Time != null)
                .Select(a => new
                {
                    WeekDay = (int)((DateTime)a.Time).DayOfWeek,
                    DeviceId = a.DeviceId,
                    Anger = a.Anger,
                    Sadness = a.Sadness,
                    Happiness = a.Happiness,
                    Surprise = a.Surprise,
                    Disgust = a.Disgust,
                    Neutral = a.Neutral
                });
            // Create the list where emotiondata is stored per hour 
            List<HourlyEmotions> weeklyEmotion = new List<HourlyEmotions>();
            double totalSum = 0;
            for (int i = 0; i < 7; i++)
            {
                var dummyVar = emotionData
                .ToList()
                .Where(a => a.WeekDay == i)
                .GroupBy(a => a.DeviceId)
                .Select(b => new
                {
                    HappinessSum = b.Sum(c => c.Happiness),
                    AngerSum = b.Sum(c => c.Anger),
                    SadnessSum = b.Sum(c => c.Sadness),
                    SurpriseSum = b.Sum(c => c.Surprise),
                    DisgustSum = b.Sum(c => c.Disgust),
                    NeutralSum = b.Sum(c => c.Neutral),
                    Count = b.Count()
                })
                .FirstOrDefault();

                if (dummyVar != null)
                {
                    totalSum = (double)(dummyVar.AngerSum 
                        + dummyVar.DisgustSum 
                        + dummyVar.HappinessSum 
                        + dummyVar.NeutralSum 
                        + dummyVar.SadnessSum 
                        + dummyVar.SurpriseSum);

                    weeklyEmotion.Add(new HourlyEmotions
                    {
                        HourOrWeekdayNumber = i,
                        Anger = (int)(dummyVar.AngerSum * 100 / totalSum),
                        Disgust = (int)(dummyVar.DisgustSum * 100 / totalSum),
                        Happiness = (int)(dummyVar.HappinessSum * 100 / totalSum),
                        Neutral = (int)(dummyVar.NeutralSum * 100 / totalSum),
                        Sadness = (int)(dummyVar.SadnessSum * 100 / totalSum),
                        Surprise = (int)(dummyVar.SurpriseSum * 100 / totalSum)
                    });
                }
                else
                {
                    weeklyEmotion.Add(new HourlyEmotions { HourOrWeekdayNumber = i });
                }
            }

            return weeklyEmotion;
        }

    }
}
