function [] = vertexUnits()
%VERTEXUNITS Displays information about the units used by VERTEX.
disp('  Units used by VERTEX: ')
disp(' ')
disp('       Electric potential:            milliVolts              (e.g. leak potential E_leak)')
disp('              Conductance:           nanoSiemens              (e.g. synaptic conductance weight)')
disp('                  Current:             picoAmps               (e.g. synaptic current weight)')
disp('      Specific resistance:     Ohm * square centimetres       (e.g. specific membrane resistance R_M)')
disp('     Specific capacitance:  micfroFarads / square centimetre  (e.g. specific membrane capacitance C)')
disp('  Longitudinal resistance:         Ohm * centimetres          (e.g. intracellular axial resistance R_A)')
disp('             Conductivity:          Siemens / metre           (e.g. extracellular conductivity sigma)')
disp('                     Time:            milliseconds            (e.g. synaptic time constant tau)')
disp('                   Length:            micrometres             (e.g. model width X)')
disp(' ')