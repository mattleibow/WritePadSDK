using System;
using Foundation;
using UIKit;

using PhatWare.WritePad;

namespace WritePadSDKiOSSample
{
    partial class OptionsViewController : UITableViewController
    {
        public Recognizer myRecognizer;

        private enum WritePadOptions
        {
            kWritePadOptionsSepLetters = 0,
            kWritePadOptionsSingleWord,
            kWritePadOptionsDictionaryOnly,
            kWritePadOptionsLearner,
            kWritePadOptionsAutcorrector,
            kWritePadOptionsUserDict,

            kWritePadOptionsTotal
        };

        public OptionsViewController(IntPtr handle)
            : base(handle)
        {
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            // return the actual number of sections
            return 1;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            // return the actual number of items in the section
            return (int)WritePadOptions.kWritePadOptionsTotal;
        }

        public override string TitleForHeader(UITableView tableView, nint section)
        {
            return null;
        }

        public override string TitleForFooter(UITableView tableView, nint section)
        {
            return null;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell("OptionsCell");

            var flags = myRecognizer.RecognitionFlags;
            var sw = new UISwitch();
            sw.ValueChanged += (object sender, EventArgs e) =>
            {
                UISwitch swit = (UISwitch)sender;
                switch ((WritePadOptions)(int)swit.Tag)
                {
                    case WritePadOptions.kWritePadOptionsSepLetters:
                        myRecognizer.RecognitionFlags = MainViewController.SetFlag(flags, swit.On, RecognitionFlags.SEPLET);
                        break;
                    case WritePadOptions.kWritePadOptionsSingleWord:
                        myRecognizer.RecognitionFlags = MainViewController.SetFlag(flags, swit.On, RecognitionFlags.SINGLEWORDONLY);
                        break;
                    case WritePadOptions.kWritePadOptionsLearner:
                        myRecognizer.RecognitionFlags = MainViewController.SetFlag(flags, swit.On, RecognitionFlags.ANALYZER);
                        break;
                    case WritePadOptions.kWritePadOptionsAutcorrector:
                        myRecognizer.RecognitionFlags = MainViewController.SetFlag(flags, swit.On, RecognitionFlags.CORRECTOR);
                        break;
                    case WritePadOptions.kWritePadOptionsUserDict:
                        myRecognizer.RecognitionFlags = MainViewController.SetFlag(flags, swit.On, RecognitionFlags.USERDICT);
                        break;
                    case WritePadOptions.kWritePadOptionsDictionaryOnly:
                        myRecognizer.RecognitionFlags = MainViewController.SetFlag(flags, swit.On, RecognitionFlags.ONLYDICT);
                        break;
                }
            };
            sw.Tag = indexPath.Row;
            cell.AccessoryView = sw;
            switch ((WritePadOptions)indexPath.Row)
            {
                case WritePadOptions.kWritePadOptionsSepLetters:
                    sw.On = flags.HasFlag(RecognitionFlags.SEPLET);
                    cell.TextLabel.Text = "Separate letters mode (PRINT)";
                    break;
                case WritePadOptions.kWritePadOptionsSingleWord:
                    sw.On = flags.HasFlag(RecognitionFlags.SINGLEWORDONLY);
                    cell.TextLabel.Text = "Disable word segmentation (single word)";
                    break;
                case WritePadOptions.kWritePadOptionsLearner:
                    sw.On = flags.HasFlag(RecognitionFlags.ANALYZER);
                    cell.TextLabel.Text = "Enable Automatic learner";
                    break;
                case WritePadOptions.kWritePadOptionsAutcorrector:
                    sw.On = flags.HasFlag(RecognitionFlags.CORRECTOR);
                    cell.TextLabel.Text = "Enable Autocorrector";
                    break;
                case WritePadOptions.kWritePadOptionsUserDict:
                    sw.On = flags.HasFlag(RecognitionFlags.USERDICT);
                    cell.TextLabel.Text = "Enable User Dictionary";
                    break;
                case WritePadOptions.kWritePadOptionsDictionaryOnly:
                    sw.On = flags.HasFlag(RecognitionFlags.ONLYDICT);
                    cell.TextLabel.Text = "Recognize Dictionary Words Only";
                    break;
            }

            return cell;
        }
    }
}
