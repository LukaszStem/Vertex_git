%[results] = generateValues(10);
%[LFPValues20] = generateValues(20);
%[LFPValues30] = generateValues(30);
%[LFPValues40] = generateValues(40);
%[LFPValues50] = generateValues(50);
%[LFPValues60] = generateValues(60);

%generateVertexVisualizerFolder('C:\Users\wassa\tdcs', Results, connections);

[rateOfChangeStim, avgPos] = ...
    calculatePlasticityValue(Results_Stim, 1:100, 1:1000, 873)

[rateOfChangeNoStim, avgPos] = ...
    calculatePlasticityValue(Results_Stim, 1:100, 1:1000, 873)

