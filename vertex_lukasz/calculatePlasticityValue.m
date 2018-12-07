function [rateOfSynapticChange, averagePosition] = calculatePlasticityValue(results, neuronIds, timeRange, layerBound) %Possibly add connectionDistance percentage? 

%Get average position of somas
positions = results.params.TissueParams.somaPositionMat(neuronIds, :);
averagePosition = [mean(positions(:, 1)), mean(positions(:, 2)), mean(positions(:, 3))];

%Get appropriate weights
weights = results.weights(neuronIds);

%Allocate for rates - last row will represent sum of colomns
rateOfSynapticChange = zeros(size(neuronIds, 2) + 1, 3);

for i=neuronIds 
    currentWeights = weights{i,1};
    rowSize = size(currentWeights,1);
    
    %Remove connections to other layers
    if(rowSize > layerBound)
        currentWeights([layerBound:rowSize],:) = [];
    end
    
    %Simply takes the difference of the mean of the weights of all
    %connections at the endtime and starttime
    for k=timeRange(2):timeRange(end)
        timeRangeIncrementMean = mean(currentWeights(:, k)) - mean(currentWeights(:, k-1));
        if timeRangeIncrementMean > 0
            rateOfSynapticChange(i, 1) = rateOfSynapticChange(i, 1) + timeRangeIncrementMean;
        else
            rateOfSynapticChange(i, 2) = rateOfSynapticChange(i, 2) + timeRangeIncrementMean;
        end
    end
    rateOfSynapticChange(i,3) = rateOfSynapticChange(i,1)+ rateOfSynapticChange(i,2);
end

%Increases in weight
rateOfSynapticChange(end, 1) = sum(rateOfSynapticChange(1:end-1, 1));

%Decreases in weight
rateOfSynapticChange(end, 2) = sum(rateOfSynapticChange(1:end-1, 2));

%Sum of all
rateOfSynapticChange(end, 3) = sum(rateOfSynapticChange(1:end-1, 3));

end

