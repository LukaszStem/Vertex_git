function [rateOfSynapticChange, averagePosition] = calculatePlasticityValue(results, neuronIds, timeRange, layerBound) %Possibly add connectionDistance percentage? 

%Get average position of somas
positions = results.params.TissueParams.somaPositionMat(neuronIds, :);
averagePosition = [mean(positions(:, 1)), mean(positions(:, 2)), mean(positions(:, 3))];

%Get appropriate weights
weights = results.weights(neuronIds);

%Allocate for rates
rateOfSynapticChange = zeros(size(neuronIds));

for (i=neuronIds) 
    currentWeights = weights{i,1};
    rowSize = size(currentWeights,1);
    
    %Remove connections to other layers
    if(rowSize > layerBound)
        currentWeights([layerBound:rowSize],:) = []
    end
    
    %Simply takes the difference of the mean of the weights of all
    %connections at the endtime and starttime
    rateOfSynapticChange(i) = mean(currentWeights(:, timeRange(end))) - mean(currentWeights(:, timeRange(1)));
end


end

