using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Countersoft.Gemini.Commons.Entity;
using Countersoft.Gemini.Extensibility.Events;
using Countersoft.Gemini.Commons.Dto;
using Countersoft.Gemini.Extensibility.Apps;
using Countersoft.Foundation.Commons.Extensions;
using Countersoft.Gemini;
using Countersoft.Gemini.Infrastructure.Managers;

namespace AutoOpenClosedItem
{
    [AppType(AppTypeEnum.Event),
    AppGuid("8338EE3A-5CBE-4AD6-82B1-E8C96EB12226"),
    AppName("Disable Future Time Logging"),
    AppDescription("Don't allow users to log time in the future")]
    public class TimeTrackingListener : IBeforeIssueTimeTrackingListener
    {
        
        public string Description { get; set; }

        public string Name { get; set; }

        public string AppGuid { get; set; }

        public IssueTimeTracking BeforeIssueTimeTrackingCreated(IssueTimeTrackingEventArgs args)
        {
            var userManager = GeminiApp.GetManager<UserManager>(args.User);
            var userDto = userManager.Convert(args.User);
            if (args.Entity.EntryDate.Date > DateTime.UtcNow.ToLocal(userDto.TimeZone).Date)
            {
                args.Cancel = true;
                args.CancelMessage = "Cannot log time in the future";
            }
            return args.Entity;
        }

        public IssueTimeTracking BeforeIssueTimeTrackingUpdated(IssueTimeTrackingEventArgs args)
        {
            return BeforeIssueTimeTrackingCreated(args);
        }

        public IssueTimeTracking BeforeIssueTimeTrackingDeleted(IssueTimeTrackingEventArgs args)
        {
            return args.Entity;
        }

        
    }
}
