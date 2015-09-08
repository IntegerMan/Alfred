#Alfred Search Specification

##Overview##

This document outlines the interfaces related to search operations, how these fit into the application, and how they work together to provide a framework for searching.

##Search Controllers##

Alfred will have a new *SearchController* property containing an object that helps manage search operations. This object will be an **ISearchController** and will be responsible for:

- Executing search queries provided from the user interface
- Sending the query to the appropriate Search Provider(s)
- Checking on long-running search operations
- Canceling search operations that have taken too long
- Logging any errors encountered during search or any queries that were aborted
- Aggregating the search results to present to the user interface
- Populating a message to show to the user to display:

	- When the search is ongoing
	- When the search has been completed, listing the count of results found
	- When the search returned no results
	- When one or more search providers encountered errors

The **ISearchController** will be provided on Alfred's initialization via *Container.Provide<ISearchController>*.

A new **AlfredSearchController** class will be created that implements this interface.

##Search Providers##

**Search Providers** are a type of *component* that *Subsystems* can provide to *Alfred*. Subsystems can provide zero, one, or many Search Provider at the time of registration.

Search Providers are components that can be searched with user input text and provide a set of zero to many Search Results related to the search text.

Each Search Provider has a *Name* that will be visible to the user in the user interface in an optional *search bar*, the *search results*, in *logging*, and in the *explorer*.

## Code Elements ##
### ISearchProvider###

Search Providers are represented in code as **ISearchProvider** instances which is defined as:


    /// <summary>
    /// Defines an <see langword="object"/> capable of providing search results
    /// </summary>
    public interface ISearchProvider
    {
    /// <summary>
    /// Executes a search operation and returns a result to track the search.
    /// </summary>
    /// <param name="searchText">The search text.</param>
    /// <returns>
    /// An <see cref="ISearchOperation" /> representing the potentially ongoing search.
    /// </returns>
    [NotNull]
    ISearchOperation PerformSearch([NotNull] string searchText);
    }
    
Because searches can be asynchronous - particularly for web-based searches or searches over large or complex data sets, each search provider returns an **ISearchOperation** representing the potentially ongoing search.

###ISearchOperation###

An **ISearchOperation** is defined as:

    public interface ISearchOperation
    {
        bool IsSearchComplete { get; }

        bool EncounteredError { get; }

        string ErrorMessage { get; }

        IEnumerable<ISearchResult> Results { get; }

        void Update();

        void Abort();
    }

Every Alfred *Update* cycle, during the **ISearchController**'s *Update* method it will check on any ongoing search operation by calling the operation's *Update* method and then checking its *IsSearchComplete* property.

Any **ISearchResult** added to **ISearchOperation**'s *Results* property will be added to the **ISearchController**'s *Results* collection as the result is added, even if the search operation's *IsSearchComplete* property is not *true*.

Once a search has completed, with or without errors, its *IsSearchComplete* property will be set to true by that **ISearchOperation**. This will tell the **ISearchController** to stop tracking the search object.

If, during the **ISearchController**'s *Update* cycle it detects that the search has taken longer than its *SearchTimeoutInMilliseconds* and there are searches that have not completed, the **ISearchController** will call each **ISearchProvider**'s *Abort* method to terminate the search.

###ISearchController##

The **ISearchController** is defined as:

    public interface ISearchController : IAlfredComponent
    {
		void PerformSearch(string searchText);
		void PerformSearch(string searchText, string providerId);

        void Abort();

        double SearchTimeoutInMilliseconds { get; set; }

        DateTime? LastSearchStart { get; }
        DateTime? LastSearchEnd { get; }
        TimeSpan? LastSearchDuration { get; }

		IEnumerable<ISearchOperation> OngoingOperations { get; }

        IEnumerable<ISearchResult> Results { get; }

        bool IsSearching { get; }

        string StatusMessage { get; }

        void Register([NotNull] ISearchProvider provider);

        IEnumerable<ISearchProvider> Providers { get; }
    }

*PerformSearch* will start a search against all providers or against a specific provider. The search will be started by calling the provider's *PerformSearch* method and tracking the result **ISearchOperation** in *OngoingOperations*.

**IAlfredComponent** demands an *Update* method that will be used to update the search and get results from any incomplete operation in *OngoingOperations*. This method will populate the search results in *Results* as the search goes on and will update *IsSearching* and *StatusMessage* with current status values. As searches complete they will be removed from *OngoingOperations*.

*Abort* is called by the user interface if the user wishes to cancel a search and will clear out the *OngoingOperations* and call *Abort* on each **ISearchOperation**.

The *SearchTimeoutInMilliseconds** will default to 30 seconds but can be configurable to any value above 0 milliseconds. Any other value will throw an **ArgumentOutOfRangeException**.

The three nullable time properties are all controlled by the **ISearchController**. When *PerformSearch* occurs, *LastSearchStart* will be set to the current time, *LastSearchEnd* and *LastSearchDuration* will be set to null. When a search is completed or aborted, *LastSearchEnd* will be set to the current time and *LastSearchDuration* will be set to the amount of time between the start and end times.

*Providers* contains the **ISearchProvider**s that are searched. This collection will be populated during *Register*. *Register* will throw an **ArgumentNullException** on null values and an **InvalidOperationException** if a provider was registered when the **ISearchController** was not *Offline*.

###ISearchResult##

Search Results represent discrete matches from a **ISearchProvider** and have bare details on the results with the intent of using these details to populate the user interface's search results list. 

**ISearchResult** is defined as:

    public interface ISearchResult
    {
        [NotNull]
        string Title { get; }

        [NotNull]
        string Description { get; }

        [NotNull]
        string LocationText { get; }

        [CanBeNull]
        Action<ISearchResult> MoreDetailsAction { get; }

        [CanBeNull]
        string MoreDetailsText { get; }
    }

*Title* is a summary title for the result and is intended to be displayed prominently in the user interface for the result.

*Description* contains some textual descriptions for the search result and are intended to give the user more context on a search result to help them decide if it is what they are looking for.

*LocationText* is a textual representation of the results location. Depending on the **ISearchProvider** this could be a URL, file path, Bible reference, street address, phone number, date / time, or some other form of identifying information related to the item.

*MoreDetailsAction* is an action that allows the user to drill into the result for more details. This should be an action that navigates the user interface, opens a web URL, opens a file folder, or runs an external application depending on the context. This action is optional, but when present is represented with a *MoreDetailsText* link.

## Chat Integration ##

Search will be added to AIML chat support via a new **SearchTagHandler** class that handles search tags. Anything inside of the search tag will be sent to **ISearchController**.*PerformSearch*. 

AIML tags will be added for the following scenarios:

- "Find [query]"
- "Find me [query]" 
- "Find [query] in [provider]"
- "Search [query]"
- "Search for [query]"
- "Search [provider] for [query]"
- "Search in [provider] for [query]"
- "Look for [query]"
- "Look for [query] in [provider]"

## Initial Search Providers ##

Initially the search providers that will be supported are:

- Bing
- Stack Overflow
- Alfred's Mind Explorer
- GitHub