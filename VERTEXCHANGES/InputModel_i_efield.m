classdef InputModel_i_efield < InputModel
  %InputModel_i_step Step input current
  %   Parameters to set in NeuronParams.Input:
  %   - amplitude, the step current amplitude (in pA). This can either be a
  %   single value for all the neurons in the group, or an array of length
  %   equal to the number of neurons in the group, specifying the
  %   amplitude per neuron.
  %   - timeOn, the simulation time (in ms) to turn the current on.
  %   - timeOff, the simulation time (in ms) to turn the current off.
  %
  %   Optional parameters to set in NeuronParams.Input:
  %   - compartmentsInput, which compartments of the neurons the current
  %   should be applied to. If not specified, the current is applied to
  %   all compartments. For standard somatic current injection,
  %   compartmentsInput should be set to 1.
  %   
  %   The current is weighted by compartment membrane area.
  
  properties (SetAccess = private)
    meanInput
  end
  
  methods
    function IM = InputModel_i_efield(N, inputID, number, timeStep, compartmentsInput, subset)
      %narginchk(4, 6)
      if nargin == 4
        compartmentsInput = 1:N.numCompartments;
        subset = 1:number;
      elseif nargin == 5
        subset = 1:number;
      end
      N.Input(inputID).amplitude = 0;
      N.Input(inputID).meanInput = N.Input(inputID).amplitude;
      IM = IM@InputModel(N, inputID, number, compartmentsInput, subset);
      IM = setupStepCurrent(IM, N, inputID, timeStep, subset);
    end
    
    function IM = setupStepCurrent(IM, N, inputID, timeStep, subset)
      
      mi = N.Input(inputID).amplitude(:);
      IM.meanInput = bsxfun(@times, mi, IM.membraneAreaRatio);
      if size(IM.meanInput, 1) > 1
        IM.meanInput = IM.meanInput(subset, :);
      end
      IM.I_input = IM.I_input .* 0;
    end
    
    
    function IM = updateInput(IM,~,activation)
        IM.I_input = activation';
    end
    
    function I = getRecordingVar(IM)
      I = IM.I_input;
    end
    
  end % methods
  
  methods(Static)
    function params = getRequiredParams()
      params = {};
    end
  end
  
end % classdef