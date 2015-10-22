// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace WritePadSDKiOSSample
{
	[Register ("MainViewController")]
	partial class MainViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIBarButtonItem clearButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		PhatWare.WritePad.InkView inkView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIBarButtonItem languageButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIBarButtonItem optionsButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIBarButtonItem recognizeAllButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextView recognizedText { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (clearButton != null) {
				clearButton.Dispose ();
				clearButton = null;
			}
			if (inkView != null) {
				inkView.Dispose ();
				inkView = null;
			}
			if (languageButton != null) {
				languageButton.Dispose ();
				languageButton = null;
			}
			if (optionsButton != null) {
				optionsButton.Dispose ();
				optionsButton = null;
			}
			if (recognizeAllButton != null) {
				recognizeAllButton.Dispose ();
				recognizeAllButton = null;
			}
			if (recognizedText != null) {
				recognizedText.Dispose ();
				recognizedText = null;
			}
		}
	}
}
