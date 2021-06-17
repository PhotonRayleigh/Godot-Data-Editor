// Author: PhotonRayleigh
// Year: 2021
// GitHub: https://github.com/PhotonRayleigh

using Godot;
using System;
using SparkLib;

namespace AutoThemes
{
    [Tool]
    public class AutoThemeIcons : Resource
    {
        [Export]
        public Texture ActionCopy = new ImageTexture();
        [Export]
        public Texture ActionCut = new ImageTexture();
        [Export]
        public Texture ActionPaste = new ImageTexture();
        [Export]
        public Texture ArrowDown = new ImageTexture();
        [Export]
        public Texture ArrowLeft = new ImageTexture();
        [Export]
        public Texture ArrowRight = new ImageTexture();
        [Export]
        public Texture ArrowUp = new ImageTexture();
        [Export]
        public Texture Back = new ImageTexture();
        [Export]
        public Texture Forward = new ImageTexture();
        [Export]
        public Texture Collapse = new ImageTexture();
        [Export]
        public Texture ColorPick = new ImageTexture();
        [Export]
        public Texture OverbrightIndicator = new ImageTexture();

        [Export]
        public Texture File = new ImageTexture();
        [Export]
        public Texture FileMediumThumb = new ImageTexture();
        [Export]
        public Texture FileBigThumb = new ImageTexture();
        [Export]
        public Texture Folder = new ImageTexture();
        [Export]
        public Texture FolderBigThumb = new ImageTexture();
        [Export]
        public Texture FolderMediumThumb = new ImageTexture();
        [Export]
        public Texture Reload = new ImageTexture();
        [Export]
        public Texture ReloadSmall = new ImageTexture();

        [Export]
        public Texture GuiChecked = new ImageTexture();
        [Export]
        public Texture GuiUnchecked = new ImageTexture();
        [Export]
        public Texture GuiRadioChecked = new ImageTexture();
        [Export]
        public Texture GuiRadioUnchecked = new ImageTexture();

        [Export]
        public Texture GuiToggleOff = new ImageTexture();
        [Export]
        public Texture GuiToggleOffMirrored = new ImageTexture();
        [Export]
        public Texture GuiToggleOn = new ImageTexture();
        [Export]
        public Texture GuiToggleOnMirrored = new ImageTexture();

        [Export]
        public Texture GuiClose = new ImageTexture();
        [Export]
        public Texture GuiCloseCustomizable = new ImageTexture();
        [Export]
        public Texture GuiDropdown = new ImageTexture();
        [Export]
        public Texture GuiEllipsis = new ImageTexture();
        [Export]
        public Texture Add = new ImageTexture();

        //Texture GuiGraphNodePort;

        [Export]
        public Texture GuiHsplitter = new ImageTexture();
        [Export]
        public Texture GuiHTick = new ImageTexture();
        [Export]
        public Texture GuiMiniCheckerboard = new ImageTexture();
        [Export]
        public Texture GuiOptionArrow = new ImageTexture();
        [Export]
        public Texture GuiProgressBar = new ImageTexture();
        [Export]
        public Texture GuiProgressFill = new ImageTexture();

        [Export]
        public Texture GuiResizer = new ImageTexture();
        [Export]
        public Texture GuiResizerMirrored = new ImageTexture();

        [Export]
        public Texture GuiScrollArrowLeft = new ImageTexture();
        [Export]
        public Texture GuiScrollArrowLeftHl = new ImageTexture();
        [Export]
        public Texture GuiScrollArrowRight = new ImageTexture();
        [Export]
        public Texture GuiScrollArrowRightHl = new ImageTexture();

        [Export]
        public Texture GuiScrollBg = new ImageTexture();
        [Export]
        public Texture GuiScrollGrabber = new ImageTexture();
        [Export]
        public Texture GuiScrollGrabberHl = new ImageTexture();
        [Export]
        public Texture GuiScrollGrabberPressed = new ImageTexture();

        [Export]
        public Texture GuiSliderGrabber = new ImageTexture();
        [Export]
        public Texture GuiSliderGrabberHl = new ImageTexture();

        [Export]
        public Texture GuiSpinboxUpdown = new ImageTexture();

        [Export]
        public Texture GuiTabMenu = new ImageTexture();
        [Export]
        public Texture GuiTabMenuHl = new ImageTexture();

        [Export]
        public Texture GuiTreeArrowDown = new ImageTexture();
        [Export]
        public Texture GuiTreeArrorLeft = new ImageTexture();
        [Export]
        public Texture GuiTreeArrowRight = new ImageTexture();
        [Export]
        public Texture GuiTreeArrowUp = new ImageTexture();
        [Export]
        public Texture GuiTreeArrowUpdown = new ImageTexture();

        [Export]
        public Texture GuiViewportHdiagsplitter = new ImageTexture();
        [Export]
        public Texture GuiViewpoertVdiagsplitter = new ImageTexture();
        [Export]
        public Texture GuiViewportVhsplitter = new ImageTexture();

        [Export]
        public Texture GuiVisibilityHidden = new ImageTexture();
        [Export]
        public Texture GuiVisibilityVisible = new ImageTexture();
        [Export]
        public Texture GuiVisibilityXray = new ImageTexture();

        [Export]
        public Texture GuiVsplitBg = new ImageTexture();
        [Export]
        public Texture GuiVsplitter = new ImageTexture();
        [Export]
        public Texture GuiVTick = new ImageTexture();

        // Text
        [Export]
        public Texture GuiSpace = new ImageTexture();
        [Export]
        public Texture GuiTab = new ImageTexture();
        [Export]
        public Texture GuiTabMirrored = new ImageTexture();

        public AutoThemeIcons()
        {
            Image emptyImage = new Image();
            emptyImage.Create(16, 16, false, Image.Format.Rgba8);
            emptyImage.Fill(Colors.Chartreuse);

            (ActionCopy as ImageTexture)!.CreateFromImage(emptyImage);
            (ActionCut as ImageTexture)!.CreateFromImage(emptyImage);
            (ActionPaste as ImageTexture)!.CreateFromImage(emptyImage);
            (ArrowDown as ImageTexture)!.CreateFromImage(emptyImage);
            (ArrowLeft as ImageTexture)!.CreateFromImage(emptyImage);
            (ArrowRight as ImageTexture)!.CreateFromImage(emptyImage);
            (ArrowUp as ImageTexture)!.CreateFromImage(emptyImage);
            (Back as ImageTexture)!.CreateFromImage(emptyImage);
            (Forward as ImageTexture)!.CreateFromImage(emptyImage);
            (Collapse as ImageTexture)!.CreateFromImage(emptyImage);
            (ColorPick as ImageTexture)!.CreateFromImage(emptyImage);
            (OverbrightIndicator as ImageTexture)!.CreateFromImage(emptyImage);

            (File as ImageTexture)!.CreateFromImage(emptyImage);
            (FileMediumThumb as ImageTexture)!.CreateFromImage(emptyImage);
            (FileBigThumb as ImageTexture)!.CreateFromImage(emptyImage);
            (Folder as ImageTexture)!.CreateFromImage(emptyImage);
            (FolderBigThumb as ImageTexture)!.CreateFromImage(emptyImage);
            (FolderMediumThumb as ImageTexture)!.CreateFromImage(emptyImage);
            (Reload as ImageTexture)!.CreateFromImage(emptyImage);
            (ReloadSmall as ImageTexture)!.CreateFromImage(emptyImage);

            (GuiChecked as ImageTexture)!.CreateFromImage(emptyImage);
            (GuiUnchecked as ImageTexture)!.CreateFromImage(emptyImage);
            (GuiRadioChecked as ImageTexture)!.CreateFromImage(emptyImage);
            (GuiRadioUnchecked as ImageTexture)!.CreateFromImage(emptyImage);

            (GuiToggleOff as ImageTexture)!.CreateFromImage(emptyImage);
            (GuiToggleOffMirrored as ImageTexture)!.CreateFromImage(emptyImage);
            (GuiToggleOn as ImageTexture)!.CreateFromImage(emptyImage);
            (GuiToggleOnMirrored as ImageTexture)!.CreateFromImage(emptyImage);
            (GuiClose as ImageTexture)!.CreateFromImage(emptyImage);
            (GuiCloseCustomizable as ImageTexture)!.CreateFromImage(emptyImage);
            (GuiDropdown as ImageTexture)!.CreateFromImage(emptyImage);
            (GuiEllipsis as ImageTexture)!.CreateFromImage(emptyImage);
            (Add as ImageTexture)!.CreateFromImage(emptyImage);

            //Texture GuiGraphNodePort;

            (GuiHsplitter as ImageTexture)!.CreateFromImage(emptyImage);
            (GuiHTick as ImageTexture)!.CreateFromImage(emptyImage);
            (GuiMiniCheckerboard as ImageTexture)!.CreateFromImage(emptyImage);
            (GuiOptionArrow as ImageTexture)!.CreateFromImage(emptyImage);
            (GuiProgressBar as ImageTexture)!.CreateFromImage(emptyImage);
            (GuiProgressFill as ImageTexture)!.CreateFromImage(emptyImage);

            (GuiResizer as ImageTexture)!.CreateFromImage(emptyImage);
            (GuiResizerMirrored as ImageTexture)!.CreateFromImage(emptyImage);

            (GuiScrollArrowLeft as ImageTexture)!.CreateFromImage(emptyImage);
            (GuiScrollArrowLeftHl as ImageTexture)!.CreateFromImage(emptyImage);
            (GuiScrollArrowRight as ImageTexture)!.CreateFromImage(emptyImage);
            (GuiScrollArrowRightHl as ImageTexture)!.CreateFromImage(emptyImage);

            (GuiScrollBg as ImageTexture)!.CreateFromImage(emptyImage);
            (GuiScrollGrabber as ImageTexture)!.CreateFromImage(emptyImage);
            (GuiScrollGrabberHl as ImageTexture)!.CreateFromImage(emptyImage);
            (GuiScrollGrabberPressed as ImageTexture)!.CreateFromImage(emptyImage);

            (GuiSliderGrabber as ImageTexture)!.CreateFromImage(emptyImage);
            (GuiSliderGrabberHl as ImageTexture)!.CreateFromImage(emptyImage);
            (GuiSpinboxUpdown as ImageTexture)!.CreateFromImage(emptyImage);

            (GuiTabMenu as ImageTexture)!.CreateFromImage(emptyImage);
            (GuiTabMenuHl as ImageTexture)!.CreateFromImage(emptyImage);

            (GuiTreeArrowDown as ImageTexture)!.CreateFromImage(emptyImage);
            (GuiTreeArrorLeft as ImageTexture)!.CreateFromImage(emptyImage);
            (GuiTreeArrowRight as ImageTexture)!.CreateFromImage(emptyImage);
            (GuiTreeArrowUp as ImageTexture)!.CreateFromImage(emptyImage);
            (GuiTreeArrowUpdown as ImageTexture)!.CreateFromImage(emptyImage);

            (GuiViewportHdiagsplitter as ImageTexture)!.CreateFromImage(emptyImage);
            (GuiViewpoertVdiagsplitter as ImageTexture)!.CreateFromImage(emptyImage);
            (GuiViewportVhsplitter as ImageTexture)!.CreateFromImage(emptyImage);

            (GuiVisibilityHidden as ImageTexture)!.CreateFromImage(emptyImage);
            (GuiVisibilityVisible as ImageTexture)!.CreateFromImage(emptyImage);
            (GuiVisibilityXray as ImageTexture)!.CreateFromImage(emptyImage);

            (GuiVsplitBg as ImageTexture)!.CreateFromImage(emptyImage);
            (GuiVsplitter as ImageTexture)!.CreateFromImage(emptyImage);
            (GuiVTick as ImageTexture)!.CreateFromImage(emptyImage);

            // Text
            (GuiSpace as ImageTexture)!.CreateFromImage(emptyImage);
            (GuiTab as ImageTexture)!.CreateFromImage(emptyImage);
            (GuiTabMirrored as ImageTexture)!.CreateFromImage(emptyImage);
        }

        // public void FixAssignments()
        // {


        //     ActionCopy = (Texture)Get(nameof(ActionCopy));
        //     ActionCut = (Texture)Get(nameof(ActionCut));
        //     ActionPaste = (Texture)Get(nameof(ActionPaste));
        //     ArrowDown = (Texture)Get(nameof(ArrowDown));
        //     ArrowLeft = (Texture)Get(nameof(ArrowLeft));
        //     ArrowRight = (Texture)Get(nameof(ArrowRight));
        //     ArrowUp = (Texture)Get(nameof(ArrowUp));
        //     Back = (Texture)Get(nameof(Back));
        //     Forward = (Texture)Get(nameof(Forward));
        //     Collapse = (Texture)Get(nameof(Collapse));
        //     ColorPick = (Texture)Get(nameof(ColorPick));
        //     OverbrightIndicator = (Texture)Get(nameof(OverbrightIndicator));

        //     File = (Texture)Get(nameof(File));
        //     FileMediumThumb = (Texture)Get(nameof(FileMediumThumb));
        //     FileBigThumb = (Texture)Get(nameof(FileBigThumb));
        //     Folder = (Texture)Get(nameof(Folder));
        //     FolderBigThumb = (Texture)Get(nameof(FolderBigThumb));
        //     FolderMediumThumb = (Texture)Get(nameof(FolderMediumThumb));
        //     Reload = (Texture)Get(nameof(Reload));
        //     ReloadSmall = (Texture)Get(nameof(ReloadSmall));

        //     GuiChecked = (Texture)Get(nameof(GuiChecked));
        //     GuiUnchecked = (Texture)Get(nameof(GuiUnchecked));
        //     GuiRadioChecked = (Texture)Get(nameof(GuiRadioChecked));
        //     GuiRadioUnchecked = (Texture)Get(nameof(GuiRadioUnchecked));

        //     GuiToggleOff = (Texture)Get(nameof(GuiToggleOff));
        //     GuiToggleOffMirrored = (Texture)Get(nameof(GuiToggleOffMirrored));
        //     GuiToggleOn = (Texture)Get(nameof(GuiToggleOn));
        //     GuiToggleOnMirrored = (Texture)Get(nameof(GuiToggleOnMirrored));
        //     GuiClose = (Texture)Get(nameof(GuiClose));
        //     GuiCloseCustomizable = (Texture)Get(nameof(GuiCloseCustomizable));
        //     GuiDropdown = (Texture)Get(nameof(GuiDropdown));
        //     GuiEllipsis = (Texture)Get(nameof(GuiEllipsis));
        //     Add = (Texture)Get(nameof(Add));

        //     //Texture GuiGraphNodePort;

        //     GuiHsplitter = (Texture)Get(nameof(GuiHsplitter));
        //     GuiHTick = (Texture)Get(nameof(GuiHTick));
        //     GuiMiniCheckerboard = (Texture)Get(nameof(GuiMiniCheckerboard));
        //     GuiOptionArrow = (Texture)Get(nameof(GuiOptionArrow));
        //     GuiProgressBar = (Texture)Get(nameof(GuiProgressBar));
        //     GuiProgressFill = (Texture)Get(nameof(GuiProgressFill));

        //     GuiResizer = (Texture)Get(nameof(GuiResizer));
        //     GuiResizerMirrored = (Texture)Get(nameof(GuiResizerMirrored));

        //     GuiScrollArrowLeft = (Texture)Get(nameof(GuiScrollArrowLeft));
        //     GuiScrollArrowLeftHl = (Texture)Get(nameof(GuiScrollArrowLeftHl));
        //     GuiScrollArrowRight = (Texture)Get(nameof(GuiScrollArrowRight));
        //     GuiScrollArrowRightHl = (Texture)Get(nameof(GuiScrollArrowRightHl));

        //     GuiScrollBg = (Texture)Get(nameof(GuiScrollBg));
        //     GuiScrollGrabber = (Texture)Get(nameof(GuiScrollGrabber));
        //     GuiScrollGrabberHl = (Texture)Get(nameof(GuiScrollGrabberHl));
        //     GuiScrollGrabberPressed = (Texture)Get(nameof(GuiScrollGrabberPressed));

        //     GuiSliderGrabber = (Texture)Get(nameof(GuiSliderGrabber));
        //     GuiSliderGrabberHl = (Texture)Get(nameof(GuiSliderGrabberHl));
        //     GuiSpinboxUpdown = (Texture)Get(nameof(GuiSpinboxUpdown));

        //     GuiTabMenu = (Texture)Get(nameof(GuiTabMenu));
        //     GuiTabMenuHl = (Texture)Get(nameof(GuiTabMenuHl));

        //     GuiTreeArrowDown = (Texture)Get(nameof(GuiTreeArrowDown));
        //     GuiTreeArrorLeft = (Texture)Get(nameof(GuiTreeArrorLeft));
        //     GuiTreeArrowRight = (Texture)Get(nameof(GuiTreeArrowRight));
        //     GuiTreeArrowUp = (Texture)Get(nameof(GuiTreeArrowUp));
        //     GuiTreeArrowUpdown = (Texture)Get(nameof(GuiTreeArrowUpdown));

        //     GuiViewportHdiagsplitter = (Texture)Get(nameof(GuiViewportHdiagsplitter));
        //     GuiViewpoertVdiagsplitter = (Texture)Get(nameof(GuiViewpoertVdiagsplitter));
        //     GuiViewportVhsplitter = (Texture)Get(nameof(GuiViewportVhsplitter));

        //     GuiVisibilityHidden = (Texture)Get(nameof(GuiVisibilityHidden));
        //     GuiVisibilityVisible = (Texture)Get(nameof(GuiVisibilityVisible));
        //     GuiVisibilityXray = (Texture)Get(nameof(GuiVisibilityXray));

        //     GuiVsplitBg = (Texture)Get(nameof(GuiVsplitBg));
        //     GuiVsplitter = (Texture)Get(nameof(GuiVsplitter));
        //     GuiVTick = (Texture)Get(nameof(GuiVTick));

        //     // Text
        //     GuiSpace = (Texture)Get(nameof(GuiSpace));
        //     GuiTab = (Texture)Get(nameof(GuiTab));
        //     GuiTabMirrored = (Texture)Get(nameof(GuiTabMirrored));
        // }

    }
}
