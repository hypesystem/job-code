namespace JobAd

{

    using System.Collections.Generic;

    using System.Linq;

 

    public class JobApplicant

    {

        public string Name { get; set; }

        public string AdditionalMotivation { get; set; }

        public List<Skill> Skills { get; set; }

        public const string ContactPersonAt3 = "Steffen Jørgensen, 31 200 953";

 

        /// <summary>

        /// Call to get applicant awesomeness level. The higher score the better.

        /// </summary>

        public int JobApplicantAwesomenessLevel()

        {

            int applicantAwesomenessLevel = 0;

 

            bool isQualified = ApplicantHasRequiredSkills();

            if (isQualified)

            {

                applicantAwesomenessLevel = 10;

                applicantAwesomenessLevel += CountNiceToHaveSkills();

                applicantAwesomenessLevel += MetaSkillBonus();

            }

            return applicantAwesomenessLevel;

        }

 

        private bool ApplicantHasRequiredSkills()

        {

            List<Skill> requiredSkills = new List<Skill> {

                                                 new Skill() { SkillName = "ASP.NET", SkillLevel = SkillLevel.AVERAGE, SkillCategory = "Programming" },

                                                 new Skill() { SkillName = "ASP.NET MVC", SkillLevel = SkillLevel.AVERAGE, SkillCategory = "Programming" },

                                                 new Skill() { SkillName = "C#", SkillLevel = SkillLevel.AVERAGE, SkillCategory = "Programming" },

                                                 new Skill() { SkillName = "Written and spoken Danish", SkillLevel = SkillLevel.PRO, SkillCategory = "Meta" }

                                             };

            foreach (Skill skill in requiredSkills)

            {

                bool hasSkill = (from relevantSkill in this.Skills

                                 where relevantSkill.SkillName == skill.SkillName

                                 && (relevantSkill.SkillLevel == SkillLevel.AVERAGE || relevantSkill.SkillLevel == SkillLevel.PRO)

                                 select relevantSkill).Any();

                if (hasSkill)

                {

                    continue;

                }

                return false;

            }

            return true;

        }

 

        private int CountNiceToHaveSkills()

        {

            List<Skill> niceToHaveSkills = new List<Skill>()

                                               {

                                                   new Skill() { SkillName = "Entity Framework", SkillLevel = SkillLevel.BEGINNER, SkillCategory = "Programming" },

                                                   new Skill() { SkillName = "SQL", SkillLevel = SkillLevel.BEGINNER, SkillCategory = "Programming" },

                                                   new Skill() { SkillName = "Unit testing", SkillLevel = SkillLevel.BEGINNER, SkillCategory = "Programming" },

                                                   new Skill() { SkillName = "Agile development (Scrum)", SkillLevel = SkillLevel.BEGINNER, SkillCategory = "Programming" },

                                                   new Skill() { SkillName = "HTML/CSS", SkillLevel = SkillLevel.BEGINNER, SkillCategory = "Programming" }

                                               };

            return (from skill in niceToHaveSkills

                    join applicantSkill in this.RelevantSkills("Programming") on skill.SkillName equals applicantSkill.SkillName

                    select skill).Count();

        }

 

        private int MetaSkillBonus()

        {

            List<Skill> metaSkills = new List<Skill>()

                                         {

                                             new Skill() { SkillName = "Enjoys fast paced environment", SkillLevel = SkillLevel.AVERAGE, SkillCategory = "Meta" },

                                             new Skill() { SkillName = "Studying on ITU or equivalent education", SkillLevel = SkillLevel.AVERAGE, SkillCategory = "Meta" },

                                             new Skill() { SkillName = "Curios about new skills and technology", SkillLevel = SkillLevel.AVERAGE, SkillCategory = "Meta" },

                                             new Skill() { SkillName = "Enjoys working closely with project managers and colleagues", SkillLevel = SkillLevel.AVERAGE, SkillCategory = "Meta" },

                                             new Skill() { SkillName = "Will work in small .NET team with senior and junior developers", SkillLevel = SkillLevel.AVERAGE, SkillCategory = "Meta" },

                                             new Skill() { SkillName = "Can work 15 hours/week", SkillLevel = SkillLevel.AVERAGE, SkillCategory = "Meta" },

                                         };

            return (from skill in metaSkills

                    join applicantSkill in this.RelevantSkills("Meta") on skill.SkillName equals applicantSkill.SkillName

                    select skill).Count();

        }

 

        private IEnumerable<Skill> RelevantSkills(string skillCategory)

        {

            return (from skill in this.Skills

                    where skill.SkillCategory == skillCategory

                    select skill).ToList();

        }

    }

 

    public class Skill

    {

        public string SkillName { get; set; }

        public string SkillCategory { get; set; }

        public SkillLevel SkillLevel { get; set; }

    }

 

    public enum SkillLevel

    {

        BEGINNER,

        AVERAGE,

        PRO

    }

}
