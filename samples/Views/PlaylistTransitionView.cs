﻿using FluidNav.Flowing;
using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Layouts;
using Sample.ViewModels;
using Sample.Views.CustomControls;
using FluidNav;

namespace Sample.Views;

public class PlaylistTransitionView : TransitionView
{
    public AbsoluteLayout _root = null!;
    private readonly Button _backButton = new();
    private readonly AvatarControl _avatar = new();
    private readonly Label _nameLabel = new();
    private readonly Label _dateLabel = new();
    private readonly Label _playlistNameLabel = new();
    private HorizontalStackLayout _dataLayout = null!;
    public readonly Label _descriptionLabel = new() { MaximumWidthRequest = 360 };
    private readonly Button _addButton = new();
    public readonly Button _downloadButton = new();
    public readonly Button _moreButton = new();

    public PlaylistTransitionView()
    {
        InitializeContent();

        var l1 = 25;
        var l2 = 95;

        var r1 = 25;

        var t1 = 25;
        var t2 = 100;

        On<PlaylistCollection>(
            _root.Flows(
                (HeightRequestProperty, 500),
                (MaximumWidthRequestProperty, 600)),
            _backButton.Flows(
                (OpacityProperty, 0)),
            _avatar.Flows(
                (MarginProperty, new Thickness(l1, t1, 0, 0)),
                (AbsoluteLayout.LayoutBoundsProperty, new Point(0, 0))),
            _nameLabel.Flows(
                (MarginProperty, new Thickness(l2, t1 + 5, 0, 0)),
                (AbsoluteLayout.LayoutBoundsProperty, new Point(0, 0))),
            _dateLabel.Flows(
                (MarginProperty, new Thickness(l2, t1 + 30, 0, 0)),
                (AbsoluteLayout.LayoutBoundsProperty, new Point(0, 0))),
            _addButton.Flows(
                (MarginProperty, new Thickness(0, t1, r1, 0)),
                (ScaleProperty, 1),
                (AbsoluteLayout.LayoutBoundsProperty, new Point(1, 0))),
            _playlistNameLabel.Flows(
                (MarginProperty, new Thickness(0, t2, 0, 0)),
                (ScaleProperty, 1),
                (AbsoluteLayout.LayoutBoundsProperty, new Point(0.5, 0))),
            _dataLayout.Flows(
                (MarginProperty, new Thickness(0, t2 + 60, 0, 0)),
                (AbsoluteLayout.LayoutBoundsProperty, new Point(0.5, 0))),
            _descriptionLabel.Flows(
                (OpacityProperty, 0)),
            _downloadButton.Flows(
                (OpacityProperty, 0)),
            _moreButton.Flows(
                (OpacityProperty, 0)));

        On<Playlist>(
            _root.Flows(
                (HeightRequestProperty, 650),
                (MaximumWidthRequestProperty, 2000)),
            _backButton.Flows(
                (OpacityProperty, 1)),
            _avatar.Flows(
                (MarginProperty, new Thickness(0, t1, 0, 0)),
                (AbsoluteLayout.LayoutBoundsProperty, new Point(0.5, 0))),
            _nameLabel.Flows(
                (MarginProperty, new Thickness(0, t2, 0, 0)),
                (AbsoluteLayout.LayoutBoundsProperty, new Point(0.5, 0))),
            _dateLabel.Flows(
                (MarginProperty, new Thickness(0, t2 + 25, 0, 0)),
                (AbsoluteLayout.LayoutBoundsProperty, new Point(0.5, 0))),
            _addButton.Flows(
                (MarginProperty, new Thickness(25, 50, 0, 0)),
                (ScaleProperty, 0.5),
                (AbsoluteLayout.LayoutBoundsProperty, new Point(0.5, 0))),
            _playlistNameLabel.Flows(
                (MarginProperty, new Thickness(0, t2 + 60, 0, 0)),
                (ScaleProperty, 1.5),
                (AbsoluteLayout.LayoutBoundsProperty, new Point(0.5, 0))),
            _dataLayout.Flows(
                (MarginProperty, new Thickness(0, t2 + 120, 0, 0)),
                (AbsoluteLayout.LayoutBoundsProperty, new Point(0.5, 0))),
            _descriptionLabel.Flows(
                (OpacityProperty, 1)),
            _downloadButton.Flows(
                (OpacityProperty, 1)),
            _moreButton.Flows(
                (OpacityProperty, 1)));
    }

    public Style<Button> ButtonStyle => new Style<Button>()
        .Add(BackgroundColorProperty, Color.FromRgba(240, 240, 240, 245))
        .Add(Button.TextColorProperty, Colors.Black)
        .Add(Button.FontSizeProperty, 18)
        .Add(Button.FontFamilyProperty, "Icons")
        .Add(Button.PaddingProperty, new Thickness(0));

    private void InitializeContent()
    {
        Content = new AbsoluteLayout()
        {
            _backButton
                .LayoutBounds(0, 0)
                .LayoutFlags(AbsoluteLayoutFlags.PositionProportional)
                .Style(ButtonStyle)
                .Bind(
                    BackgroundColorProperty,
                    getter: (PlaylistVM vm) => vm.BackgroundColor)
                .Bind(
                    Button.TextColorProperty,
                    getter: (PlaylistVM vm) => vm.TextColor)
                .Size(40)
                .Text("<")
                .TapGesture(() => FlowNavigation.Current.GoBack()),

            _avatar
                .LayoutFlags(AbsoluteLayoutFlags.PositionProportional)
                .Bind(
                    AvatarControl.AvatarImageProperty,
                    getter: (PlaylistVM vm) => vm.AuthorPicture),

            _nameLabel
                .LayoutFlags(AbsoluteLayoutFlags.PositionProportional)
                .Style(new Style<Label>()
                    .Add(Label.TextColorProperty, Color.FromRgba(50, 50, 50, 250))
                    .Add(Label.FontSizeProperty, 16))
                .Bind(
                    Label.TextProperty,
                    getter: (PlaylistVM vm) => vm.Author)
                .Bind(
                    Label.TextColorProperty,
                    getter: (PlaylistVM vm) => vm.TextColor),

            _dateLabel
                .LayoutFlags(AbsoluteLayoutFlags.PositionProportional)
                .Style(new Style<Label>()
                    .Add(Label.TextColorProperty, Colors.Black)
                    .Add(Label.FontSizeProperty, 12)
                    .Add(OpacityProperty, 0.75d))
                .Bind(
                    Label.TextProperty,
                    getter: (PlaylistVM vm) => vm.Date,
                    convert: date => date.ToString("MMMM yyyy"))
                .Bind(
                    Label.TextColorProperty,
                    getter: (PlaylistVM vm) => vm.TextColor),

            _addButton
                .LayoutFlags(AbsoluteLayoutFlags.PositionProportional)
                .Style(ButtonStyle)
                .Size(40)
                .Text("+"),

            _playlistNameLabel
                .LayoutFlags(AbsoluteLayoutFlags.PositionProportional)
                .FontSize(32)
                .Bold()
                .Center()
                .Margins(top: 20, bottom: 25)
                .Bind(
                    Label.TextProperty,
                    getter: (PlaylistVM vm) => vm.Name)
                .Bind(
                    Label.TextColorProperty,
                    getter: (PlaylistVM vm) => vm.TextColor),

            new HorizontalStackLayout()
            {
                new Label()
                    .ZIndex(1)
                    .Margins(right: 10)
                    .Font(family: "Icons")
                    .FontSize(14)
                    .Text("2")
                    .Bind(
                        Label.TextColorProperty,
                        getter: (PlaylistVM vm) => vm.TextColor),
                new Label()
                    .FontSize(14)
                    .Text("8,908")
                    .Bind(
                        Label.TextColorProperty,
                        getter: (PlaylistVM vm) => vm.TextColor),

                new Label()
                    .Margins(left: 20, right: 20)
                    .FontSize(14)
                    .Text("-")
                    .Bind(
                        Label.TextColorProperty,
                        getter: (PlaylistVM vm) => vm.TextColor),

                new Label()
                    .Margins(right: 10)
                    .Font(family: "Icons")
                    .FontSize(14)
                    .Text("4")
                    .Bind(
                        Label.TextColorProperty,
                        getter: (PlaylistVM vm) => vm.TextColor),
                new Label()
                    .FontSize(14)
                    .Text("6h 40m")
                    .Bind(
                        Label.TextColorProperty,
                        getter: (PlaylistVM vm) => vm.TextColor)
            }
            .Ref(out _dataLayout)
            .LayoutFlags(AbsoluteLayoutFlags.PositionProportional)
            .Center()
            .Margins(bottom: 30),

             _descriptionLabel
                .LayoutBounds(0.5, 0.4)
                .LayoutFlags(AbsoluteLayoutFlags.PositionProportional)
                .FontSize(16)
                .Bind(
                    Label.TextProperty,
                    getter: (PlaylistVM vm) => vm.Description)
                .Bind(
                    Label.TextColorProperty,
                    getter: (PlaylistVM vm) => vm.TextColor),

             new Image()
                .LayoutBounds(0.5, 1)
                .LayoutFlags(AbsoluteLayoutFlags.PositionProportional)
                .Aspect(Aspect.AspectFill)
                .Size(300)
                .Bind(
                    Image.SourceProperty,
                    getter: (PlaylistVM vm) => vm.Banner),

             new Button()
                .LayoutBounds(0.5, 0.9)
                .LayoutFlags(AbsoluteLayoutFlags.PositionProportional)
                .Style(ButtonStyle)
                .Size(70)
                .FontSize(20)
                .Text("6"),

            new Button()
                .LayoutBounds(0.5, 0.9)
                .LayoutFlags(AbsoluteLayoutFlags.PositionProportional)
                .Style(ButtonStyle)
                .BackgroundColor(Color.FromRgba(0, 0, 0, 255))
                .TextColor(Colors.White)
                .Opacity(0.85)
                .Margins(right: 160, bottom: 10)
                .Size(50)
                .FontSize(16)
                .Text("0"),

            new Button()
                .LayoutBounds(0.5, 0.9)
                .LayoutFlags(AbsoluteLayoutFlags.PositionProportional)
                .Style(ButtonStyle)
                .BackgroundColor(Color.FromRgba(0, 0, 0, 255))
                .TextColor(Colors.White)
                .Opacity(0.85)
                .Margins(left: 160, bottom: 10)
                .Size(50)
                .FontSize(16)
                .Text("5"),

            _downloadButton
                .LayoutBounds(0.5, 0.9)
                .LayoutFlags(AbsoluteLayoutFlags.PositionProportional)
                .Style(ButtonStyle)
                .BackgroundColor(Color.FromRgba(0, 0, 0, 255))
                .TextColor(Colors.White)
                .Opacity(0.85)
                .Margins(right: 300, bottom: 10)
                .Size(50)
                .FontSize(16)
                .Text("1"),

            _moreButton
                .LayoutBounds(0.5, 0.9)
                .LayoutFlags(AbsoluteLayoutFlags.PositionProportional)
                .Style(ButtonStyle)
                .BackgroundColor(Color.FromRgba(0, 0, 0, 255))
                .TextColor(Colors.White)
                .Opacity(0.85)
                .Margins(left: 300, bottom: 10)
                .Size(50)
                .FontSize(16)
                .Text("3")

        }
        .Ref(out _root)
        .Bind(
            BackgroundColorProperty,
            getter: (PlaylistVM vm) => vm.BackgroundColor);

#if MACCATALYST || IOS
        // BUG, the Tapped is not working on MacCatalyst
        _backButton.Clicked += (s, e) => FlowNavigation.Current.GoBack();
#endif

    }
}
