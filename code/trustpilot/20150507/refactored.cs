using System;
using System.Collections.Generic;

namespace Trustpilot
{
    public class AttractiveCompanyFactory
    {
        public static Company GetAttractiveCompany()
        {
            var website = GetInterestingCompanyWebsite("trustpilot.com");
        
            var company = new Company("Trustpilot", website);
            
            var technologies = GetAttractiveTechnologies();
            var benefits = GetAttractiveBenefits();
            var positions = GetReallyCoolJobs();
            
            company.Technologies.Union(technologies);
            company.Benefits.Union(benefits);
            company.AvailablePositions.Union(positions);
            
            return company;
        }
        
        private static Website GetInterestingCompanyWebsite(string baseUri)
        {
            return new Website() {
                AvaiableJobsUri = GetUriFromBase("https://jobs.{0}/", baseUri);
                BackendDeveloperChallengeUri = GetUriFromBase("https://followthewhiterabbit.{0}/", baseUri);
                FrontendDeveloperChallengeUri = GetUriFromBase("https://followthewhiterabbit.{0}/fe/index.html", baseUri);
            };
        }
        
        private static GetUriFromBase(uriTemplate, baseUri)
        {
            return new Uri(String.Format(uriTemplate, baseUri));
        }
        
        public static IEnumerable<string> GetAttractiveTechnologies()
        {
            return new string[] {
                ".NET",
                "C#",
                "F#",
                "Node.js",
                "Angular.js",
                "SASS",
                "MongoDB",
                "NewRelic",
                "Docker",
                "AllCloud-AllTheWay"
                // And many more
            };
        }
        
        public static IEnumerable<string> GetAttractiveBenefits()
        {
            return new string[] {
                "20% time",
                "Hackathons",
                "Minimal process",
                "Product owning and autonomous teams",
                "Really talented people",
                "Great parties",
                "Conference budget",
                "Opportunities to grow"
            };
        }
        
        public static IEnumerable<string> GetReallyCoolJobs()
        {
            return new string[] {
                "Backend developers",
                "Frontend developers",
                "Product managers",
                "Product specialists",
                "Data scientists",
                "UX specialist",
                "Cloud engineers"
            };
        }
    }
    
    public class CompanyWebsite
    {
        public Uri AvailableJobsUri { get; private set; }
        public Uri BackendCodingChallengeUri { get; private set; }
        public Uri FrontendCodingChallengeUri { get; private set; }
        
        public void OpenJobApplicationUri()
        {
            OpenUri(AvailableJobsUri);
        }

        public void OpenBackendCodingChallengeUri()
        {
            OpenUri(BackendCodingChallengeUri);
        }

        public void OpenFrontendCodingChallengeUri()
        {
            OpenUri(FrontendCodingChallengeUri);
        }
        
        private void OpenUri(Uri uri)
        {
            System.Diagnostics.Process.Start(uri.ToString());
        }
    }
    
    public class Company
    {
        public HashSet<string> Technologies { get; private set; }
        public HashSet<string> Benefits { get; private set; }
        public HashSet<string> AvailablePositions { get; private set; }
        
        public string Name { get; private set }
        public CompanyWebsite Website { get; private set; }
        
        public Company(string name, CompanyWebsite website)
        {
            Name = name;
            Website = website;
            Technologies = new HashSet<string>();
            Benefits = new HashSet<string>();
            AvailablePositions = new HashSet<string>();
        }
    }
}
