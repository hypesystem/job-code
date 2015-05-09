Trustpilot held a GeekBeer event for the Copenhagen .NET Users Group (CNUG) at Nørrebro Bryghus on 2015-05-07 where they advertised available jobs.
They had two challenges: a frontend challenge and a backend challenge.
To get the advertising across, they handed out job listings written in C#.

![Photo of job listing](screenshot.png?raw=true)

The following is a review of the code.
The original code can be read [here](original.cs).

-----

### Move dependencies out

Dependencies for a file are not often significant, but it should be easy to get an overview.
That is best achieved by placing the `using` statements in the start of the file, not inside the `namespace` (as the `namespace` is significant, following it with rarely-significant information is just a nuisance).

### Indentation and consistency.

Let's start simple.
Look at the elements in the lists.
How much are they indented?
Well, they're indented enough that the curly `{` is under the `L` in `List`.
That `L` happens to be at different places depending on the variable name.
That's really silly, and breaks the rhythm of reading.
The indentation should be consistent, so let's conform it to the same style as for the methods:
the curly is placed as the first character on the next line.

Notice that the first list, `TechnologiesWeUse`, differs slightly from the others:
the last "element" in it is actually a comment, `// And many more`.
But at the same time, the element before it ends with a comma `,`.
This compiles and all is fine, but it is inconsistent with the other lists.
The trailing comma is often left in, so version control systems only see one changed line when elements are added or removed.
On the other hand, it implies that something is missing.
Either decision can make sense depending on code style.
Being inconsistent never makes sense, and only adds confusion.

```CSharp
//Before
var l = new List<string>
{
    "Hello",
};

//After
var l = new List<string>
{
    "Hello",
    "World, // <-- only line changed
};
```

### Where\* and What\* methods

Moving on to the next low-hanging fruit:
starting methods with `Where` and `What`.
Imagine using these in code---it's not pretty.

```CSharp
var linkToJobListings = technology.WhereShouldIGoToApplyForJob();
```

They are obviously getters, and could easily be renamed as such:

```CSharp
public Uri GetJobApplicationUri() { /* ... */ }
public Uri GetDirectLinkToBackendCodingChallenge() { /* ... */ }
public Uri GetDirectLinkToFrontendCodingChallenge() { /* ... */ }
```

But let's take a step back.
What can these methods actually be used for?
They are used to send the user to the website; that's what we want to do.
As it turns out, there is actually a method for that:
[`System.Diagnostics.Process.Start`](https://msdn.microsoft.com/en-US/library/53ezey2s.aspx).

```CSharp
public void OpenJobApplicationUri() {
    System.Diagnostics.Process.Start("https://jobs.trustpilot.com/");
}
```

This is a bit unclear, so lets extract a method (in case we choose to later change *how* we open Uris):

```CSharp
public void OpenJobApplicationUri() {
    OpenUri("https://jobs.trustpilot.com/");
}

private void OpenUri(string uri) {
    System.Diagnostics.Process.Start(uri);
}
```

The same can be done for all the other `Uri` methods.

### The `dynamic` keyword, properties

`dynamic` is a type specifier that means the variable can take any content at runtime.
It basically means that no guarantees as to content of a variable are made.
Seeing as we're writing C#, it seems a shame to not take advantage of the type-system, especially in this case, where the properties are always set to the same type of value, `List<string>`.

We don't want to be too specific, though: if we decide to use something other than a list in the future, that should be possible without breaking the interface.
Let's go for ´IEnumberable&lt;string&gt;`s.

In addition, the technologies shouldn't be modifiable from the outside; let's make the setters private.

```CSharp
public IEnumerable<string> TechnologiesWeUse { get; private set; }
public IEnumerable<string> BenefitsWeHave { get; private set; }
public IEnumerable<string> PeopleWeAreLookingFor { get; private set; }
```

### Classes

Now we're getting to a big problem.
Try and write some code using this class, even with its improvements:

```CSharp
var technology = new Trustpilot.Technology();
var technologies = technology.TechnologiesWeUse; // <-- returns null

technology.WhatIsTrustpilotAbout();
technologies = technology.TechnologiesWeUse; // <-- returns the technologies

technology.OpenJobApplicationUri(); // <-- at least this works
```

None of the methods really make sense on the object we're calling them.
Why does a `Technology` have `TechnologiesWeUse`?
Why does a `Technology` have calls to very specific `Uri`s?

Let's get thinking.
What we're really describing here is actually a company, Trustpilot.
I'll add a constructor.

```CSharp
namespace Trustpilot
{
    public class Company
    {
        // ...

        public string Name { get; private set; }

        public Company(string name) {
            Name = name;
        }

        // ...
    }
}
```

We can now create a new company with a name.

We need to decide on where the `BenefitsWeHave`, `TechnologiesWeUse`, etc. come from.
Are they given from the outside (set after creation) or inherent to the company (set *at* creation)?
The correct answer is probably both:
a company starts with a certain set, but they may change over time.
That means the lists we expose should not be overwritable, but should be mutable.
We have to abandon our `IEnumerable` idea from before---I'll replace it with a `Set`, because that accurately captures our need to store unique strings in an order that is not significant.

In the same breath, let us rename the collections so they make sense:
`TechnologiesWeUse` becomes simply `Technologies` (use is implied by the field belonging to the company);
`BenefitsWeHave` becomes simply `Benefits`; and
`PeopleWeAreLookingFor` becomes `AvailablePositions`.

Let us also instantiate these collections in the constructor, so they are never `null`.

```CSharp
public class Company
{
    public HashSet<string> Technologies { get; private set; }
    public HashSet<string> Benefits { get; private set; }
    public HashSet<string> AvailablePositions { get; private set; }
    
    public string Name { get; private set }
    
    public Company(string name)
    {
        Name = name;
        Technologies = new HashSet<string>();
        Benefits = new HashSet<string>();
        AvailablePositions = new HashSet<string>();
    }
}
```

We can remove the `WhatIsTrustpilotAbout` code, and instead write some code that uses the `Company` class:

```CSharp
var technologies = new string[]
{
    ".NET",
    // ...
};

var benefits = new string[]
{
    "20% time",
    // ...
};

var positions = new string[]
{
    "Backend developers",
    // ...
};

var company = new Company("Trustpilot");

company.Technologies.Union(technologies);
company.Benefits.Union(benefits);
company.AvailablePositions.Union(positions);
```

Okay, there's clearly something that should be extracted here; the code is quite long.
Let's first look at the `Uri`s once more, though.
We still have some weird code:

```CSharp
company.OpenAvailableJobsUri();
```

As it turns out, these all belong to a completely different concept: a website.

```CSharp
public class CompanyWebsite
{
    public Uri AvailableJobsUri { get; private set; }
    public Uri BackendCodingChallengeUri { get; private set; }
    public Uri FrontendCodingChallengeUri { get; private set; }
    
    public void OpenAvailableJobsUri() { /* ... */ }
    //...
}
```

Great. That was simple.
The website needs to belong to the company, so we'll add another property:

```CSharp
public class Company(string name, CompanyWebsite website)
{
    Name = name;
    Website = website;
    // ...
}
```

We can now write the using code:

```CSharp
public Company GetAttractiveCompany()
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

private Website GetInterestingCompanyWebsite(string baseUri)
{
    return new Website()
    {
        AvaiableJobsUri = GetUriFromBase("https://jobs.{0}/", baseUri);
        BackendDeveloperChallengeUri = GetUriFromBase("https://followthewhiterabbit.{0}/", baseUri);
        FrontendDeveloperChallengeUri = GetUriFromBase("https://followthewhiterabbit.{0}/fe/index.html", baseUri);
    };
}

private GetUriFromBase(uriTemplate, baseUri)
{
    return new Uri(String.Format(uriTemplate, baseUri));
}

public IEnumerable<string> GetAttractiveTechnologies()
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

public IEnumerable<string> GetAttractiveBenefits()
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

public IEnumerable<string> GetReallyCoolJobs()
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
```

Is the code longer?
Yes.
But it is also more extendable, readable, and useful.
At the same time, the message that Trustpilot is an interesting company using cool technologies, offering great benefits, and wanting to challenge you isn't lost.
