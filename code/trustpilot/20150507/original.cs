namespace Trustpilot
{
    using System;
    using System.Collections.Generic;

    public class Technology
    {
        public dynamic TechnologiesWeUse { get; set; }

        public dynamic BenefitsWeHave { get; set; }

        public dynamic PeopleWeAreLookingFor { get; set; }

        public void WhatIsTrustpilotAbout()
        {
            TechnologiesWeUse = new List<string>
                                    {
                                        ".NET",
                                        "C#",
                                        "F#",
                                        "Node.js",
                                        "Angular.js",
                                        "SASS",
                                        "MongoDB",
                                        "NewRelic",
                                        "Docker",
                                        "AllCloud-AllTheWay",
                                        // And many more
                                    };

            BenefitsWeHave = new List<string>
                                 {
                                     "20% time",
                                     "Hackathons",
                                     "Minimal process",
                                     "Product owning and autonomous teams",
                                     "Really talented people",
                                     "Great parties",
                                     "Conference budget",
                                     "Opportunities to grow"
                                 };

            PeopleWeAreLookingFor = new List<string>
                                        {
                                            "Backend developers",
                                            "Frontend developers",
                                            "Product managers",
                                            "Product specialists",
                                            "Data scientists",
                                            "UX specialist",
                                            "Cloud engineers"
                                        };
        }
        
        public Uri WhereShouldIGoToApplyForJob()
        {
            return new Uri("https://jobs.trustpilot.com/");
        }

        public Uri WhatIsTheDirectLinkToTheBackendCodingChallenge()
        {
            return new Uri("https://followthewhiterabbit.trustpilot.com/");
        }

        public Uri WhatIsTheDirectLinkToTheFrontendCodingChallenge()
        {
            return new Uri("https://followthewhiterabbit.trustpilot.com/fe/index.html");
        }
    }
}
