# FluidNav

Fluid nav is an experimental alternative to maui `Shell` it also uses a URI-based navigation experience that uses routes to navigate to any page in the app, it helps to build soft transitions between views.

### Why?

- The MAUI default shell is hard to customize, with this control you can use any XAML view and use it as a host of your app.
- it is inspired on the [web transitions api](https://developer.chrome.com/docs/web-platform/view-transitions/) idea, and the example provided in this repo is based on [this](https://live-transitions.pages.dev/).

### Disclaim

It is an experiment, use it at your own risk, the performance seems great (using the Release build) even with "large" collections, because it uses a `CollectionView` to display the data.

![mobile transition](https://github.com/beto-rodriguez/FluidNav/assets/10853349/be82f21b-fe41-4586-b07a-bf5000447fdf)

![desktop transition](https://github.com/beto-rodriguez/FluidNav/assets/10853349/f755f5c1-9b88-44ad-9777-d4a5b6cc0d0b)
