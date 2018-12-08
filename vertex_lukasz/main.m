dirPath = "C:\Users\b8054586\Desktop\"
randomSeeds = [50, 100, 150, 200]
neuronIdSets = [1, 873]

% for i = 1:size(randomSeeds, 2)
%     fileName = strcat(dirPath, num2str(randomSeeds(i)), "_");
%     
%     % With stimulation
%     generateValues(true, neuronIdSets(1):neuronIdSets(2), randomSeeds(i), ...
%         strcat(fileName, num2str(neuronIdSets(1)), "-", num2str(neuronIdSets(2)), "Stim"));
%     clearvars -except i dirPath randomSeeds neuronIdSets fileName
%     
%     % Without stimulation
%     generateValues(false, neuronIdSets(1):neuronIdSets(2), randomSeeds(i), ...
%         strcat(fileName, num2str(neuronIdSets(1)), "-", num2str(neuronIdSets(2)), "NoStim"));
%     clearvars -except i dirPath randomSeeds neuronIdSets fileName
% end

numberOfPoints = 100;
resolution = 10000/numberOfPoints;

currentStartTime = 0;

stimVec = ["Stim", "NoStim"];

rates = zeros([2, size(randomSeeds, 2), numberOfPoints-1, 3]);

for i = 1:2
    for j = 1:size(randomSeeds, 2)
        fileName = strcat(dirPath, num2str(randomSeeds(j)), "_");
        resultsFile = strcat(fileName, num2str(neuronIdSets(1)), "-", num2str(neuronIdSets(2)), stimVec(i), ".mat");
        connectionsFile = strcat(fileName, num2str(neuronIdSets(1)), "-", num2str(neuronIdSets(2)), stimVec(i), "Connections.mat");
        load(resultsFile);
        load(connectionsFile);
        for k=1:numberOfPoints-1
            startTime = resolution * k;
            [rateOfChangeStim, avgPos] = ...
                calculatePlasticityValue(Results, 1:873, connections, startTime:startTime + resolution, 873);
            
            rates(i, j, k, 1) = rateOfChangeStim(end,1);
            rates(i, j, k, 2) = rateOfChangeStim(end,2);
            rates(i, j, k, 3) = rateOfChangeStim(end,3);
        end
        
        clearvars Results connections
    end
end


%generateVertexVisualizerFolder('C:\Users\wassa\smalltest', Results, connections);

%[rateOfChangeStim, avgPos] = ...
%    calculatePlasticityValue(Results_Stim, 1:100, 1:1000, 873)

%[rateOfChangeNoStim, avgPos] = ...
%    calculatePlasticityValue(Results_Stim, 1:100, 1:1000, 873)

