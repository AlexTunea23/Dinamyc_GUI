   M S B A M L   `   `   `  ���� |  xSystem.Windows.Controls.DataVisualization.Toolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35�rclr-namespace:System.Windows.Controls.DataVisualization;assembly=System.Windows.Controls.DataVisualization.Toolkit)System.Windows.Controls.DataVisualization  �{clr-namespace:System.Windows.Controls.DataVisualization.Charting;assembly=System.Windows.Controls.DataVisualization.Toolkit2System.Windows.Controls.DataVisualization.Charting   GUI   GUI.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     �DVrclr-namespace:System.Windows.Controls.DataVisualization;assembly=System.Windows.Controls.DataVisualization.Toolkit     �chartingToolkit{clr-namespace:System.Windows.Controls.DataVisualization.Charting;assembly=System.Windows.Controls.DataVisualization.Toolkit       � Title$	  GUI��$	��350��$	��525��.��� .+��� .@��� $��Hardware��.��� ���	#FFE5E5E5$��0,4,0,-4q� J� RenderTransformOrigin$ 0.5,0.5&� � ColumnDefinitions �� $	��65* ��� $
��446* �z�V� .~��� �� ��  �� Angle$ 0.016Q�T� .+��� -   $��buttonConnect��$��Connect��$
��Left=�$��53.045,104,0,0q�$	��Top=�$	��199��$��20��$��1���� -   $��Ports��$
��Left=�$��52.045,22,0,0q�$	��Top=�$	��200�� J� 	AllowDrop . $��1���� -   $��Bauds��$
��Left=�$��52.045,49,0,0q�$	��Top=�$	��200��$��1���� $��Select Port��$
��Left=�$��	23,22,0,0q�$	��Top=�$��2���� $��Select Baud Rate��$
��Left=�$��	23,48,0,0q�$	��Top=�$��2���� -   $��sendPeriodA1��$��
Set Period��$
��Left=�$��177.045,129,0,0q�$	��Top=�$��75��$��23��$��1���� -   $��	sendBoxA1��$
��Left=�$��23��$��52.045,129,0,0q� �� TextWrapping$
 Wrap=�$	��Top=�$	��120��$��1���� -   $��receiveBlock��
�������$
��Left=�$��278.045,213,0,0q�$
��Wrap=�$	��Top=�$��46��$	��119��$��1���� $��
Timer Send��$
��Left=�$��
19,128,0,0q�$	��Top=�$��2���� $��Sensitivity��$
��Left=�$��
23,182,0,0q�$	��Top=�$��2���� -   $��tresholdBox��$
��Left=�$��23��$��53.045,185,0,0q�$
 Wrap=�$	��Top=�$	��120��$��1���� -   $��sendPeriodA0��$��
Set Period��$
��Left=�$��177.045,157,0,0q�$	��Top=�$��75��$��23��$��1���� -	    ��Name$ 	sendBoxA0��$
��Left=�$��23��$��53.045,157,0,0q�$
 Wrap=�$	��Top=�$	��120��$��1���� $��Timer Acquisition��$
��Left=�$��
19,156,0,0q�$	��Top=�$��2���� -
    ��Name$ sendTreshold��$	��Set��$
��Left=�$��177.045,185,0,0q�$	��Top=�$��75��$��23��$��1���� -   $��StartAcquisition��$��Start Acquisition��$
��Left=�$��52.045,213,0,0q�$	��Top=�$	��200��$��21��$��1���� -   $��StopAcquisition��$��Stop Acquisition��$
��Left=�$��52.045,239,0,0q�$	��Top=�$	��200��$��1���� -   $��	axisTitle��$
��Left=�$��23��$��53.045,76,0,0q�$
 Wrap=�$	��Top=�$	��199��$��1���� $��ChartingAxis��$
��Left=�$��	23,76,0,0q�$	��Top=�$��2���� -   $��
dataMissed��
�������$
��Left=�$��278,129,0,0q�$
��Wrap=�$	��Top=�$��46��$	��119��$��1���� $��Missing data in chart
��$
��Left=�$��278.045,184,0,0q�$	��Top=�$ 0.5,0.5&�$	��158��$��1��z�V� .~��� �� �� $ 0.495Q�T� �� $��Serial corrupted data��$
��Left=�$��278.045,98,0,0q�$	��Top=�$��1���� $��Live Charting�� � 	Resources    InvisibleDataPoint0	 �� 
TargetTypeB   <System.Windows.Controls.DataVisualization.Charting.DataPoint
 �� Property �� Value  InvisibleDataPoint1  InvisibleDataPoint2��%�   &	        &	 ;     &	 v     �� "	  .��� 
 � ��
 ��  ��� 
 � �� Z� �� "	  .��� 
 � ��
 �  ���� 
 � �� Z� �� "	  .��� 
 � ��
 � ����� 
 � �� Z� .��� ���	#FFE5E5E5 �� $	��65* ��� $
��446* �.+�>   8System.Windows.Controls.DataVisualization.Charting.Chart  -     Name$ 	lineChart��   Title$  ��$��10q�$��1��
����  �$��2��   PlotAreaStyle �� "	 �.��� 
 � ��
 �   �   Series. E   ?System.Windows.Controls.DataVisualization.Charting.ColumnSeries  $	��350��
���   �H   BSystem.Windows.Controls.DataVisualization.Charting.DataPointSeries   ItemsSource ��  �� Path$&  MyCurrentResultsView.ResultsView�   IndependentValueBinding �� *Key+   DependentValueBinding �� *Value+   DataPointStyle �� H   BSystem.Windows.Controls.DataVisualization.Charting.ColumnDataPoint"	  .��� 
 � �� ��  �� 
TargetType"  .R�� .+��� #��z��
���   �� $��	0 -20 0 0q�$��Center=�$	��Top=�.+���    FormattedDependentValue#��z $��2q��� -   $��buttonStart��$��Start��$��1��$��Right=�$��	0,65,25,0q�$��1��$	��Top=�$��85���� -   $��maxValue��
�������$
��Left=�$��
155,19,0,0q�$��1��$
��Wrap=�$��25��$	��156��$	��Top=�$��1���� -    ��Name$ minValue��
�������$
��Left=�$��
155,49,0,0q�$��1��$
��Wrap=�$��23��$	��156��$	��Top=�$��1���� $��	Maximum 
��$��79,19,296,0q�$��1��$	��Top=�$��26��$��1���� $��Minimum��$��79,48,296,0q�$��1��$	��Top=�$��26��$��1���� -   $��checkDataPoint��$��Data Points
��$��Right=�$��	0,19,25,0q�$��1��$��1��$��85��$	��Top=�$��18���� -   $��dataPointNumber��$��1��$��Right=�$��23��$��	0,37,25,0q�$��1��$
 Wrap=�$	��Top=�$��85�� � RowDefinitions �� $
��Auto ��� 