////////////////////////////////////////////////////////////////////////
//
// This file is part of gmic-sharp-pdn-example, an example Paint.NET
// Effect plugin that uses the gmic-sharp-pdn library.
//
// Copyright (c) 2020 Nicholas Hayes
//
// This file is licensed under the MIT License.
// See LICENSE.txt for complete licensing and attribution information.
//
////////////////////////////////////////////////////////////////////////

using GmicSharpPdn;
using PaintDotNet;
using PaintDotNet.AppModel;
using PaintDotNet.Effects;
using PaintDotNet.IndirectUI;
using PaintDotNet.PropertySystem;
using System.Drawing;
using System.Globalization;

namespace GmicSharpPdnExample
{
    [PluginSupportInfo(typeof(PluginSupportInfo))]
    public sealed class GmicSharpPdnExamplePlugin : PropertyBasedEffect
    {
        public GmicSharpPdnExamplePlugin()
            : base("GmicSharpPdn Example", null, SubmenuNames.Distort, new EffectOptions { Flags = EffectFlags.Configurable })
        {
        }

        private enum PropertyNames
        {
            Amplitude,
            Smoothness,
            Angle
        }

        protected override PropertyCollection OnCreatePropertyCollection()
        {
            Property[] props = new Property[]
            {
                new Int32Property(PropertyNames.Amplitude, 30, 0, 200),
                new DoubleProperty(PropertyNames.Smoothness, 1.5, 0.0, 10.0),
                new DoubleProperty(PropertyNames.Angle, 45.0, 0.0, 360.0)
            };

            return new PropertyCollection(props);
        }

        protected override ControlInfo OnCreateConfigUI(PropertyCollection props)
        {
            ControlInfo configUI = CreateDefaultConfigUI(props);

            configUI.SetPropertyControlValue(PropertyNames.Amplitude, ControlInfoPropertyNames.DisplayName, "Amplitude");
            configUI.SetPropertyControlValue(PropertyNames.Smoothness, ControlInfoPropertyNames.DisplayName, "Smoothness");
            configUI.SetPropertyControlValue(PropertyNames.Angle, ControlInfoPropertyNames.DisplayName, "Angle");
            configUI.SetPropertyControlType(PropertyNames.Angle, PropertyControlType.AngleChooser);

            return configUI;
        }

        protected override void OnCustomizeConfigUIWindowProperties(PropertyCollection props)
        {
            // Add help button to effect UI
            props[ControlInfoPropertyNames.WindowTitle].Value = "GmicSharpPdn Example - G'MIC Water";
            props[ControlInfoPropertyNames.WindowHelpContentType].Value = WindowHelpContentType.PlainText;
            props[ControlInfoPropertyNames.WindowHelpContent].Value = "GmicSharpPdnExample v0.1\nCopyright © 2020 by Nicholas Hayes and David Tschumperlé \nAll rights reserved.";

            base.OnCustomizeConfigUIWindowProperties(props);
        }

        protected override void OnSetRenderInfo(PropertyBasedEffectConfigToken newToken, RenderArgs dstArgs, RenderArgs srcArgs)
        {
            int amplitude = newToken.GetProperty<Int32Property>(PropertyNames.Amplitude).Value;
            double smoothness = newToken.GetProperty<DoubleProperty>(PropertyNames.Smoothness).Value;
            double angle = newToken.GetProperty<DoubleProperty>(PropertyNames.Angle).Value;

            using (PdnGmicSharp gmic = new PdnGmicSharp())
            {
                // Add the source surface as the first input image.
                using (PdnGmicBitmap source = new PdnGmicBitmap(srcArgs.Surface))
                {
                    gmic.AddInputImage(source);
                }

                string command = string.Format(CultureInfo.InvariantCulture,
                                               "water[0] {0},{1},{2}",
                                               amplitude.ToString(CultureInfo.InvariantCulture),
                                               smoothness.ToString(CultureInfo.InvariantCulture),
                                               angle.ToString(CultureInfo.InvariantCulture));

                // Run the G'MIC command and allow it to receive cancellation info from Paint.NET.
                gmic.RunGmic(command, () => this.IsCancelRequested);

                if (gmic.Error != null)
                {
                    this.Services.GetService<IExceptionDialogService>().ShowErrorDialog(null, gmic.Error);
                }
                else if (!gmic.Canceled)
                {
                    Surface output = gmic.Output?.Surface;

                    if (output != null)
                    {
                        // Copy the rendered G'MIC image to the destination surface,
                        // and restrict the copied portions to the user's selection.
                        dstArgs.Surface.CopySurface(output, this.EnvironmentParameters.GetSelectionAsPdnRegion());
                    }
                }
            }

            base.OnSetRenderInfo(newToken, dstArgs, srcArgs);
        }

        protected override void OnRender(Rectangle[] renderRects, int startIndex, int length)
        {
            // All work is performed in OnSetRenderInfo.
        }
    }
}
