import sys
import time
import numpy as np
from shapely.geometry import Polygon, MultiPolygon
#from problem_solution import Item, Container, Problem, Solution
import copy
import itertools
import random
#from problem_solution import Solution, get_bounds
from scipy.interpolate import interpolate
from shapely import affinity
import matplotlib.pyplot as plt
from shapely.geometry import LineString
#rom shape_functions import *
import UnityEngine


# types of problems that can be solved: Knapsack-Packing Joint Problem, or Packing Problem
KNAPSACK_PACKING_PROBLEM_TYPE = "KnapsackPacking"
PACKING_PROBLEM_TYPE = "Packing"

# directory where to save figures and results of the problems created specifically for the Knapsack-Packing Joint Problem
# KNAPSACK_PACKING_PROBLEM_DIR = "../Output/Problems/CustomKnapsackPacking/Comparison/"
KNAPSACK_PACKING_PROBLEM_DIR = "C:/Users/fruzs/Code/Szakdolgozat/knapsack-packing/Output/Problems/CustomKnapsackPacking/Comparison/"

# directory where to save figures and results of instances of the Packing Problem
PACKING_PROBLEM_DIR = "../Output/Problems/CustomKnapsackPacking/Comparison/"


def create_problems(dot_coordinates):
    tuple_list = [(int(dot_coordinates[i]), int(dot_coordinates[i + 1])) for i in range(0, len(dot_coordinates), 2)]
    container_shape = Polygon(tuple_list)
    problems, solutions = list(), list()
    start_time = time.time()

    max_weight = np.inf

    container = Container(max_weight, container_shape)

    items = [Item(Polygon([(0., 0.), (40., 0.), (40., 80.), (0., 80.)]), 1., 1.),
             Item(Polygon([(0., 0.), (40., 0.), (40., 80.), (0., 80.)]), 1., 1.),
             Item(Polygon([(0., 0.), (40., 0.), (40., 80.), (0., 80.)]), 1., 1.),
             Item(Polygon([(0., 0.), (40., 0.), (40., 80.), (0., 80.)]), 1., 1.),
             Item(Polygon([(0., 0.), (40., 0.), (40., 80.), (0., 80.)]), 1., 1.),
             Item(Polygon([(0., 0.), (40., 0.), (40., 80.), (0., 80.)]), 1., 1.),
             Item(Polygon([(0., 0.), (40., 0.), (40., 80.), (0., 80.)]), 1., 1.),
             Item(Polygon([(0., 0.), (40., 0.), (40., 80.), (0., 80.)]), 1., 1.),
             ]

    problem = Problem(container, items)
    problems.append(problem)

    solution = Solution(problem)
    solutions.append(solution)

    return problems, [str(i + 1) for i in range(len(problems))], solutions


def execute_algorithm_with_params(params):
    """Execute the algorithm specified in the first of the passed parameters with the rest of parameters, and return the solution, value and elapsed time"""

    # unpack the algorithm and its parameters
    algorithm, algorithm_name, problem, show_solution_plot, solution_plot_save_path, calculate_times, calculate_value_evolution = params

    value_evolution = None
    times_dict = None
    if calculate_times:
        solution: object
        solution = algorithm(problem)
    else:
        solution = algorithm(problem)

    if solution and (show_solution_plot or solution_plot_save_path):
        solution.visualize(show_plot=show_solution_plot, save_path=solution_plot_save_path)

    coordinates = solution.get_coordinates()
    # print(coordinates)
    return solution, solution.value, value_evolution, times_dict


def execute_algorithm(algorithm, algorithm_name, problem, show_solution_plot=False, solution_plot_save_path=None,
                      calculate_times=False, calculate_fitness_stats=False, execution_num=1, process_num=1):
    """Execute the passed algorithm as many times as specified (with each execution in a different CPU process if indicated), returning (at least) lists with the obtained solutions, values and elapsed times (one per execution)"""

    # encapsulate the algorithm and its parameters in a tuple for each execution (needed for multi-processing)
    param_tuples = [(algorithm, algorithm_name, problem, show_solution_plot, solution_plot_save_path, calculate_times,
                     calculate_fitness_stats) for _ in range(execution_num)]

    solutions, values, value_evolutions, times, time_divisions = list(), list(), list(), list(), list()

    for i in range(execution_num):
        solution, value, value_evolution, time_division = execute_algorithm_with_params(param_tuples[i])
        solutions.append(solution)
        values.append(value)
        value_evolutions.append(value_evolution)
        time_divisions.append(time_division)

    return solutions, values, value_evolutions, times, time_divisions


def perform_experiments(dot_coordinates):
    """Perform a set of experiments for the problem with the passed index, and producing output in the specified directory (when applicable)"""

    # data structure where to store all the problems, and the experimental results for each algorithm: solutions, final values and times
    experiment_dict = dict()

    # perform experiments if pre-existing results were not loaded
    if not experiment_dict:
        # print(dot_coordinates)
        # given a problem type, create a set of problem instances that are solved (optimally) with manual placements (using actions available for all the algorithms)
        problems, problem_names, manual_solutions = create_problems(dot_coordinates)

        if problems and problem_names and manual_solutions:

            # parameters for the experimentation; note: calculating internal times and value evolution can increase the overall time of algorithms (in a slight, almost neglectible way)
            execution_num = 1  # 1
            process_num = 1  # 1
            calculate_internal_times = True
            calculate_value_evolution = False

            start_time = time.time()

            # solve each problem with each algorithm
            for i, (problem, problem_name, solution) in enumerate(zip(problems, problem_names, manual_solutions)):

                experiment_dict[problem_name] = {"problem": problem, "manual_solution": solution, "algorithms": dict()}

                # solve the problem with different algorithms, executing each one multiple times to gain statistical significance
                # for (algorithm_name, algorithm) in [("Greedy", greedy.solve_problem), ("Reversible", reversible.solve_problem), ("Evolutionary", evolutionary.solve_problem)]:

                for (algorithm_name, algorithm) in [("Greedy", solve_problem)]:
                    solutions, values, value_evolutions, times, time_divisions = execute_algorithm(algorithm=algorithm,
                                                                                                   algorithm_name=algorithm_name,
                                                                                                   problem=problem,
                                                                                                   execution_num=execution_num,
                                                                                                   process_num=process_num,
                                                                                                   calculate_times=calculate_internal_times,
                                                                                                   calculate_fitness_stats=calculate_value_evolution)
                    experiment_dict[problem_name]["algorithms"][algorithm_name] = {"solutions": solutions,
                                                                                   "values": values,
                                                                                   "value_evolutions": value_evolutions,
                                                                                   "times": times,
                                                                                   "time_divisions": time_divisions}

            # show the total time spent doing experiments (note that significant overhead can be introduced beyond calculation time if plots are shown or saved to files; for strict time measurements, plotting should be avoided altogether)

    else:
        print("The experiments cannot be performed (there are no problems available).")

# default maximum number of iterations for the greedy algorithm
MAX_ITER_NUM = 100000

# default maximum number of iterations without any change to perform early stopping
MAX_ITER_NUM_WITHOUT_CHANGES = 30000

# default number of repetitions of the algorithm
REPETITION_NUM = 1

def select_item(items_with_profit_ratio):

    """Given a list of tuples of the form (item_index, item_profitability_ratio, item), select an item proportionally to its profitability ratio, and return its index"""

    # find the cumulative profitability ratios, code based on random.choices() from the standard library of Python
    cumulative_profit_ratios = list(itertools.accumulate(item_with_profit_ratio[1] for item_with_profit_ratio in items_with_profit_ratio))
    profit_ratio_sum = cumulative_profit_ratios[-1]

    # randomly select a ratio within the range of the sum
    profit_ratio = random.uniform(0, profit_ratio_sum)

    # find the value that lies within the random ratio selected; binary search code is based on bisect.bisect_left from standard Python library, but adapted to profitability ratio check
    lowest = 0
    highest = len(items_with_profit_ratio)
    while lowest < highest:
        middle = (lowest + highest) // 2
        if cumulative_profit_ratios[middle] <= profit_ratio:
            lowest = middle + 1
        else:
            highest = middle

    return lowest


def solve_problem(problem, max_iter_num=MAX_ITER_NUM, max_iter_num_without_changes=MAX_ITER_NUM_WITHOUT_CHANGES, repetition_num=REPETITION_NUM, item_index_to_place_first=-1, return_value_evolution=False):

    """Find and return a solution to the passed problem, using a greedy strategy"""

    # determine the bounds of the container
    min_x, min_y, max_x, max_y = get_bounds(problem.container.shape)

    if return_value_evolution:
        value_evolution = list()
    else:
        value_evolution = None

    # discard the items that would make the capacity of the container to be exceeded
    #original_items_by_weight = original_items_by_weight[:get_index_after_weight_limit(original_items_by_weight, problem.container.max_weight)]
    original_items_by_index = [(index_item_tuple[0], 1, index_item_tuple[1]) for index_item_tuple in sorted(list(problem.items.items()), key=lambda index_item_tuple: index_item_tuple[0])]

    best_solution = None

    # if the algorithm is iterated, it is repeated and the best solution is kept in the end
    for _ in range(repetition_num):

        # if the algorithm is iterated, use a copy of the initial sorted items, to start fresh next time
        if repetition_num > 1:
            ##items_by_weight = copy.deepcopy(original_items_by_weight)
            items_by_index = copy.deepcopy(original_items_by_index)
        else:
            ##items_by_weight = original_items_by_weight
            items_by_index = original_items_by_index
        # create an initial solution with no item placed in the container
        solution = Solution(problem)

        # placements can only be possible with capacity and valid items
        #if problem.container.max_weight and items_by_index:
        if items_by_index:
            iter_count_without_changes = 0

            # try to add items to the container, for a maximum number of iterations
            for i in range(max_iter_num):

                #select item by index

                list_index = select_item(items_by_index)
                item_index = items_by_index[list_index][0]

                # try to add the item in a random position, if it is valid, remove the item from the pending list

                if(problem.start_position[0] >= max_x-10):
                    problem.start_position = 11.0, problem.start_position[1] + 21
                else:
                    problem.start_position = problem.start_position[0] + 1, problem.start_position[1]

                if solution.add_item(item_index, problem.start_position, random.uniform(min_y, max_y)):
                    check = solution.problem.items[item_index]

                    # remove the placed item from the list of pending items
                    if list_index >= 0:
                        items_by_index.pop(list_index)

                    # if focusing on an item to place first, find its associated entry in the list to remove it
                    else:
                        for list_i in range(len(items_by_index)):
                            if items_by_index[list_i][0] == item_index:
                                items_by_index.pop(list_i)
                                break

                    # stop early if it is not possible to place more items, because all have been placed
                    if not items_by_index:
                        break

                    # reset the potential convergence counter, since an item has been added
                    iter_count_without_changes = 0

                else:
                    # register the fact of being unable to place an item this iteration
                    iter_count_without_changes += 1

                    # stop early if there have been too many iterations without changes (unless a specific item is tried to be placed first)
                    if iter_count_without_changes >= max_iter_num_without_changes and item_index_to_place_first < 0:
                        break

                if return_value_evolution:
                    value_evolution.append(solution.value)

        # if the algorithm uses multiple iterations, adopt the current solution as the best one if it is the one with highest value up to now
        if not best_solution or solution.value > best_solution.value:
            best_solution = solution

    if return_value_evolution:
        return best_solution, value_evolution

    return best_solution


class PlacedShape(object):

    """Class representing a geometric shape placed in a container with a reference position and rotation"""

    __slots__ = ("shape", "position", "rotation")

    def __init__(self, shape, position=(0., 0.), rotation=0., move_and_rotate=True):

        """Constructor"""

        # the original shape should remain unchanged, while this version is moved and rotated in the 2D space
        self.shape = copy_shape(shape)

        # the original points of the shape only represented distances among them, now the center of the bounding rectangle of the shape should be found in the reference position
        self.position = position
        if move_and_rotate:
            self.update_position(position)
            bounding_rectangle_center = get_bounding_rectangle_center(self.shape)
            self.move((position[0] - bounding_rectangle_center[0], position[1] - bounding_rectangle_center[1]), False)

        # rotate accordingly to the specified angle
        self.rotation = rotation
        if move_and_rotate:
            self.rotate(rotation, False)

    def __deepcopy__(self, memo=None):

        """Return a deep copy"""

        # the constructor already deep-copies the shape
        return PlacedShape(self.shape, copy.deepcopy(self.position), self.rotation, False)

    def update_position(self, new_position):

        """Update the position"""

        self.position = new_position

    def move(self, displacement, update_reference_position=True):

        """Move the shape as much as indicated by the displacement"""

        shape_to_move = self.shape

        # only move when it makes sense
        if displacement != (0., 0.):

            shape_to_move = affinity.translate(shape_to_move, displacement[0], displacement[1])

            self.shape = shape_to_move

            if update_reference_position:
                self.update_position((self.position[0] + displacement[0], self.position[1] + displacement[1]))

    def move_to(self, new_position):

        """Move the shape to a new position, updating its points"""

        self.move((new_position[0] - self.position[0], new_position[1] - self.position[1]))

    def rotate(self, angle, update_reference_rotation=True, origin=None):

        """Rotate the shape around its reference position according to the passed rotation angle, expressed in degrees"""

        # only rotate when it makes sense
        if not np.isnan(angle) and angle != 0 and (origin is not None):

            shape_to_rotate = self.shape

            if not origin:
                origin = self.position
            shape_to_rotate = affinity.rotate(shape_to_rotate, angle, origin)

            self.shape = shape_to_rotate

            if update_reference_rotation:
                self.rotation += angle

    def rotate_to(self, new_rotation):

        """Rotate the shape around its reference position so that it ends up having the passed new rotation"""

        self.rotate(new_rotation - self.rotation)


class Item(object):

    """Class representing an item that can be added to the container of a problem"""

    __slots__ = ("shape", "weight", "value")

    def __init__(self, shape, weight, value):

        """Constructor"""

        self.shape = shape
        self.weight = weight
        self.value = value

    def __deepcopy__(self, memo=None):

        """Deep copy"""

        return Item(copy_shape(self.shape), self.weight, self.value)


class Container(object):

    """Class representing a container in a problem, defined by its shape and maximum allowed weight"""

    __slots__ = ("max_weight", "shape")

    def __init__(self, max_weight, shape):

        """Constructor"""

        self.max_weight = max_weight
        self.shape = shape


class Problem(object):

    """Class representing an instance of the Two-Dimensional Irregular Shape Packing Problem combined with the Knapsack Problem"""

    __slots__ = ("container", "items", "start_position")

    def __init__(self, container, items):

        """Constructor"""

        self.container = container
        self.items = {index: item for index, item in enumerate(items)}
        self.start_position = (10, 20)


class Solution(object):

    """Class representing a feasible solution to a problem, specified with a set of item placements, with their position and rotation in the container"""

    __slots__ = ("problem", "placed_items", "weight", "value")

    def __init__(self, problem, placed_items=None, weight=0., value=0.):
        __items__ = placed_items
        """Constructor"""

        self.problem = problem
        self.placed_items = placed_items if placed_items else dict()
        self.weight = weight
        self.value = value

    def __deepcopy__(self, memo=None):

        """Return a deep copy"""

        # deep-copy the placed items
        return Solution(self.problem, {index: copy.deepcopy(placed_item) for index, placed_item in self.placed_items.items()}, self.weight, self.value)

    def is_valid_placement(self, item_index):

        """Return whether this solution is valid considering only the item with the specified index and its relation with the rest of items, which is the case only when the placed items do not exceed the capacity of the container, and there are no intersections between items or between an item and the container"""

        # the weight of the item must not cause an exceed of the container's capacity
        if self.weight <= self.problem.container.max_weight:

            shape = self.placed_items[item_index].shape

            # the item must be completely contained in the container
            if does_shape_contain_other(self.problem.container.shape, shape):

                # the item's shape is not allowed to intersect with any other placed item's shape
                for other_index, other_placed_shape in self.placed_items.items():

                    if item_index != other_index:

                        if do_shapes_intersect(shape, other_placed_shape.shape):

                            return False

                return True

        return False

    def get_area(self):

        """Return the sum of the area of the placed items"""

        return sum(placed_shape.shape.area for _, placed_shape in self.placed_items.items())

    def get_global_bounds(self):

        """Return the extreme points of the shape defining the global bounding rectangle"""

        global_min_x = global_min_y = np.inf
        global_max_x = global_max_y = -np.inf

        for _, placed_shape in self.placed_items.items():

            min_x, min_y, max_x, max_y = get_bounds(placed_shape.shape)

            if min_x < global_min_x:
                global_min_x = min_x

            if min_y < global_min_y:
                global_min_y = min_y

            if max_x > global_max_x:
                global_max_x = max_x

            if max_y > global_max_y:
                global_max_y = max_y

        return global_min_x, global_min_y, global_max_x, global_max_y

    def get_global_bounding_rectangle_area(self):

        """Return the area of the rectangle defined by the extreme points of the shape"""

        # find the extreme points defining the global bounding rectangle
        min_x, min_y, max_x, max_y = self.get_global_bounds()

        # return the area of the bounding rectangle
        return abs(min_x - max_x) * abs(min_x - max_y)

    def get_random_placed_item_index(self, indices_to_ignore=None):

        """Randomly select and return an index of a placed item, excluding those to ignore"""

        # get the indices of placed items, discarding those that should be ignored
        if not indices_to_ignore:
            valid_placed_item_indices = list(self.placed_items.keys())
        else:
            valid_placed_item_indices = [item_index for item_index in self.placed_items.keys() if item_index not in indices_to_ignore]

        # there may be no valid item
        if not valid_placed_item_indices:
            return None

        # return a randomly selected index
        return random.choice(valid_placed_item_indices)

    def _add_item(self, item_index, position, rotation):

        """Place the problem's item with the specified index in the container in the passed position and having the specified rotation, without checking if it leads to an invalid solution"""

        # the item is marked as placed, storing information about the position and rotation of the shape
        self.placed_items[item_index] = PlacedShape(self.problem.items[item_index].shape, position, rotation)

        # update the weight and value of the container in the current solution
        self.weight += self.problem.items[item_index].weight
        self.value += self.problem.items[item_index].value

    def add_item(self, item_index, position, rotation=np.nan):

        """Attempt to place the problem's item with the specified index in the container in the passed position and having the specified rotation, and return whether it was possible or otherwise would have lead to an invalid solution"""

        # the index of the item must be valid and the item cannot be already present in the container
        if 0 <= item_index < len(self.problem.items) and item_index not in self.placed_items:

            item = self.problem.items[item_index]

            # the weight of the item must not cause an exceed of the container's capacity
            if self.weight + item.weight <= self.problem.container.max_weight:


                # temporarily insert the item in the container, before intersection checks
                self._add_item(item_index, position, rotation)

                # ensure that the solution is valid with the new placement, i.e. it causes no intersections
                if self.is_valid_placement(item_index):

                    return True

                # undo the placement if it makes the solution unfeasible
                else:

                    self.remove_item(item_index)

        return False

    def remove_item(self, item_index):

        """Attempt to remove the item with the passed index from the container, and return whether it was possible, i.e. whether the item was present in the container before removal"""

        if item_index in self.placed_items:

            # stop considering the weight and value of the item to remove
            self.weight -= self.problem.items[item_index].weight
            self.value -= self.problem.items[item_index].value

            # the item stops being placed
            del self.placed_items[item_index]

            return True

        return False

    def remove_random_item(self):

        """Attempt to remove one of the placed items from the container, selecting it randomly, and return the index of the removed item, or -1 if the container is empty"""

        # if the container is empty, an item index cannot be returned
        if self.weight > 0:

            # choose an index randomly
            removal_index = self.get_random_placed_item_index()

            # perform the removal
            if self.remove_item(removal_index):
                return removal_index

        return .1

    def _move_item(self, item_index, displacement, has_checked_item_in_container=False):

        """Move the item with the passed index as much as indicated by the displacement, without checking if it leads to an invalid solution"""

        if has_checked_item_in_container or item_index in self.placed_items:

            self.placed_items[item_index].move(displacement)

    def move_item(self, item_index, displacement):

        """Attempt to move the item with the passed index as much as indicated by the displacement, and return whether it was possible"""

        if item_index in self.placed_items:

            old_position = self.placed_items[item_index].position

            # temporarily move the item, before intersection checks
            self._move_item(item_index, displacement, True)

            # ensure that the solution is valid with the new movement, i.e. it causes no intersections
            if self.is_valid_placement(item_index):

                return True

            # undo the movement if it makes the solution unfeasible
            else:

                self._move_item_to(item_index, old_position, True)

        return False

    def _move_item_to(self, item_index, new_position, has_checked_item_in_container=False):

        """Move the item with the passed index to the indicated new position, without checking if it leads to an invalid solution"""

        if has_checked_item_in_container or item_index in self.placed_items:

            self.placed_items[item_index].move_to(new_position)

    def move_item_to(self, item_index, new_position):

        """Attempt to move the item with the passed index to the indicated new position, and return whether it was possible"""

        if item_index in self.placed_items:

            old_position = self.placed_items[item_index].position

            # temporarily move the item, before intersection checks
            self._move_item_to(item_index, new_position)

            # ensure that the solution is valid with the new movement, i.e. it causes no intersections
            if self.is_valid_placement(item_index):

                return True

            # undo the movement if it makes the solution unfeasible
            else:

                self._move_item_to(item_index, old_position)

        return False

    def move_item_in_direction(self, item_index, direction, point_num, min_dist_to_check, max_dist_to_check, has_checked_item_in_container=False):

        """Try to move the item with the passed index in the passed (x, y) direction, as far as possible without intersecting, checking as many points as indicated"""

        # at least one point should be checked
        if point_num >= 1:

            if has_checked_item_in_container or item_index in self.placed_items:

                placed_item = self.placed_items[item_index]

                # normalize the direction
                norm = np.linalg.norm(direction)
                direction = (direction[0] / norm, direction[1] / norm)

                # create a line that goes through the reference position of the item and has the passed direction
                line = LineString([placed_item.position, (placed_item.position[0] + direction[0] * max_dist_to_check, placed_item.position[1] + direction[1] * max_dist_to_check)])

                # find the intersection points of the line with other placed items or the container
                intersection_points = list()
                intersection_points.extend(get_intersection_points_between_shapes(line, self.problem.container.shape))
                for other_index, other_placed_shape in self.placed_items.items():
                    if item_index != other_index:
                        intersection_points.extend(get_intersection_points_between_shapes(line, other_placed_shape.shape))

                # at least an intersection should exist
                if intersection_points:

                    # find the smallest euclidean distance from the item's reference position to the first point of intersection
                    intersection_point, min_dist = min([(p, np.linalg.norm((placed_item.position[0] - p[0], placed_item.position[1] - p[1]))) for p in intersection_points], key=lambda t: t[1])

                    # only proceed if the two points are not too near
                    if min_dist >= min_dist_to_check:
                        points_to_check = list()

                        # if there is only one point to check, just try that one
                        if point_num == 1:
                            return self.move_item_to(item_index, intersection_point)

                        # the segment between the item's reference position and the nearest intersection is divided in a discrete number of points
                        iter_dist = min_dist / point_num
                        for i in range(point_num - 1):
                            points_to_check.append((placed_item.position[0] + direction[0] * i * iter_dist, placed_item.position[1] + direction[1] * i * iter_dist))
                        points_to_check.append(intersection_point)

                        # perform binary search to find the furthest point (among those to check) where the item can be placed in a valid way; binary search code is based on bisect.bisect_left from standard Python library, but adapted to perform placement attempts
                        has_moved = False
                        nearest_point_index = 1
                        furthest_point_index = len(points_to_check)
                        while nearest_point_index < furthest_point_index:
                            middle_point_index = (nearest_point_index + furthest_point_index) // 2
                            if self.move_item_to(item_index, points_to_check[middle_point_index]):
                                nearest_point_index = middle_point_index + 1
                                has_moved = True
                            else:
                                furthest_point_index = middle_point_index

                        return has_moved

        return False

    def move_and_rotate_item(self, item_index, displacement, angle):

        """Try to move the item with the passed index according to the placed displacement and rotate it as much as indicated by the passed angle"""

        if item_index in self.placed_items:

            old_position = self.placed_items[item_index].position
            old_rotation = self.placed_items[item_index].rotation

            # temporarily move and rotate the item, before intersection checks
            self._move_item(item_index, displacement, True)
            self._rotate_item(item_index, angle, True)

            # ensure that the solution is valid with the new movement and rotation, i.e. it causes no intersections
            if self.is_valid_placement(item_index):

                return True

            # undo the movement and rotation if it makes the solution unfeasible
            else:

                self._move_item_to(item_index, old_position, True)
                self._rotate_item_to(item_index, old_rotation, True)

        return False

    def move_and_rotate_item_to(self, item_index, new_position, new_rotation):

        """Try to move and rotate the item with the passed index so that it has the indicated position and rotation"""

        if item_index in self.placed_items:

            old_position = self.placed_items[item_index].position
            old_rotation = self.placed_items[item_index].rotation

            # temporarily move and rotate the item, before intersection checks
            self._move_item_to(item_index, new_position, True)
            self._rotate_item_to(item_index, new_rotation, True)

            # ensure that the solution is valid with the new movement and rotation, i.e. it causes no intersections
            if self.is_valid_placement(item_index):

                return True

            # undo the movement and rotation if it makes the solution unfeasible
            else:

                self._move_item_to(item_index, old_position, True)
                self._rotate_item_to(item_index, old_rotation, True)

        return False

    def swap_placements(self, item_index0, item_index1, swap_position=True, swap_rotation=True):

        """Try to swap the position and/or the rotation of the two items with the passed indices"""

        # at least position and rotation should be swapped
        if swap_position or swap_rotation:

            # the two items need to be different and placed in the container
            if item_index0 != item_index1 and item_index0 in self.placed_items and item_index1 in self.placed_items:

                # keep track of the original position and rotation of the items
                item0_position = self.placed_items[item_index0].position
                item1_position = self.placed_items[item_index1].position
                item0_rotation = self.placed_items[item_index0].rotation
                item1_rotation = self.placed_items[item_index1].rotation

                # swap position if needed, without checking for validity
                if swap_position:

                    self._move_item_to(item_index0, item1_position, True)
                    self._move_item_to(item_index1, item0_position, True)

                # swap rotation if needed, without checking for validity
                if swap_rotation:

                    self._rotate_item_to(item_index0, item1_rotation, True)
                    self._rotate_item_to(item_index1, item0_rotation, True)

                # ensure that the solution is valid with the swapped movement and/or rotation, i.e. it causes no intersections
                if self.is_valid_placement(item_index0) and self.is_valid_placement(item_index1):

                    return True

                # undo the movement and rotation if it makes the solution unfeasible
                else:

                    # restore position if it was changed
                    if swap_position:

                        self._move_item_to(item_index0, item0_position, True)
                        self._move_item_to(item_index1, item1_position, True)

                    # restore rotation if it was changed
                    if swap_rotation:

                        self._rotate_item_to(item_index0, item0_rotation, True)
                        self._rotate_item_to(item_index1, item1_rotation, True)

        return False

    def get_items_inside_item(self, item_index):

        """Return the indices of the items that are inside the item with the passed index"""

        inside_item_indices = list()

        if item_index in self.placed_items:

            item = self.placed_items[item_index]

            # only multi-polygons can contain other items
            if type(item.shape) == MultiPolygon:

                holes = list()
                for geom in item.shape.geoms:
                    holes.extend(Polygon(hole) for hole in geom.interiors)

                for other_index, placed_shape in self.placed_items.items():

                    if other_index != item_index:

                        for hole in holes:

                            if does_shape_contain_other(hole, self.placed_items[other_index].shape):

                                inside_item_indices.append(other_index)
                                break

        return inside_item_indices
    def get_coordinates(self):
        coordinates = list()
        for k in self.placed_items.keys():
            coordinates.append(self.placed_items[k].position)
        #print("COORDINATES:")
        print(coordinates)
        return coordinates
    def visualize(self, title_override=None, show_title=True, show_container_value_and_weight=True, show_outside_value_and_weight=True, show_outside_items=True, color_items_by_profit_ratio=True, show_item_value_and_weight=True, show_value_and_weight_for_container_items=False, show_reference_positions=False, show_bounding_boxes=False, show_value_weight_ratio_bar=True, force_show_color_bar_min_max=False, show_plot=True, save_path=None):

        """Visualize the solution, with placed items in their real position and rotation, and the other ones visible outside the container"""

        can_consider_weight = self.problem.container.max_weight != np.inf

        # set up the plotting figure
        fig_size = (13, 6.75)
        dpi = 160
        fig = plt.figure(figsize=fig_size, dpi=dpi)
        if show_outside_items:
            ax1 = fig.add_subplot(1, 2, 1)
            ax1.set(aspect="equal")
            ax2 = fig.add_subplot(1, 2, 2, sharex=ax1, sharey=ax1)
            ax2.set(aspect="equal")
            ax2.tick_params(axis="both", which="major", labelsize=11)
        else:
            ax1 = plt.gca()
            ax1.set(aspect="equal")
            ax2 = None
        ax1.tick_params(axis="both", which="major", labelsize=11)
        if show_title:
            fig.suptitle(title_override if title_override else "2D Irregular Shape Packing + 0/1 Knapsack Problem")

        outside_item_bounds = dict()
        total_outside_item_width = 0.

        # represent the container
        x, y = get_shape_exterior_points(self.problem.container.shape, True)
        container_color = (.8, .8, .8)
        boundary_color = (0., 0., 0.)
        ax1.plot(x, y, color=boundary_color, linewidth=1)
        ax1.fill(x, y, color=container_color)
        empty_color = (1., 1., 1.)
        if type(self.problem.container.shape) == MultiPolygon:
            for geom in self.problem.container.shape.geoms:
                for hole in geom.interiors:
                    x, y = get_shape_exterior_points(hole, True)
                    fill_color = empty_color
                    boundary_color = (0., 0., 0.)
                    ax1.plot(x, y, color=boundary_color, linewidth=1)
                    ax1.fill(x, y, color=fill_color)

        font = {'family': 'serif', 'color':  'black', 'weight': 'normal', 'size': 12}

        # show the total value and weight in the container, and the maximum acceptable weight (capacity)
        if show_container_value_and_weight:
            value_weight_string = "V={}".format(self.value if can_consider_weight else int(self.value))
            if can_consider_weight:
                value_weight_string += ", W={}, Wmax={}".format(self.weight, self.problem.container.max_weight)
            ax1.set_title("Items inside the container\n({})".format(value_weight_string), fontsize=13)

        # determine the range of item profitability ratio, for later coloring of items
        min_profit_ratio = np.inf
        max_profit_ratio = -np.inf
        item_profit_ratios = dict()
        for item_index, item in self.problem.items.items():
            if item.weight == 0:
                profit_ratio = np.inf
            else:
                profit_ratio = item.value / item.weight
            item_profit_ratios[item_index] = profit_ratio
            min_profit_ratio = min(min_profit_ratio, profit_ratio)
            max_profit_ratio = max(max_profit_ratio, profit_ratio)
        best_profit_color = (1, 0.35, 0)
        worst_profit_color = (1, 0.8, 0.8)
        color_interp = interpolate.interp1d([min_profit_ratio, max_profit_ratio], [0, 1])


        """
        # if possible, add a color-bar showing the value/weight ratio scale
        if show_value_weight_ratio_bar:
            fig.subplots_adjust(bottom=0.15)
            fig.subplots_adjust(wspace=0.11)
            bar_x, bar_y, bar_width, bar_height = 0.5, 0.1, 0.3, 0.02
            bar_ax = fig.add_axes([bar_x - bar_width * 0.5, bar_y - bar_height * 0.5, bar_width, bar_height])
            color_map = LinearSegmentedColormap.from_list(name="profit-colors", colors=[worst_profit_color, best_profit_color])
            norm = colors.Normalize(vmin=min_profit_ratio, vmax=max_profit_ratio)
            if force_show_color_bar_min_max:
                ticks = np.linspace(min_profit_ratio, max_profit_ratio, 7, endpoint=True)
            else:
                ticks = None
            bar = colorbar.ColorbarBase(bar_ax, cmap=color_map, norm=norm, ticks=ticks, orientation='horizontal', ticklocation="bottom")
            bar.set_label(label="value/weight ratio", size=13)
            bar.ax.tick_params(labelsize=11)
        """



        for item_index, item in self.problem.items.items():

            # represent the placed items

            if item_index in self.placed_items:

                if color_items_by_profit_ratio:
                    fill_color = worst_profit_color + tuple(best_profit_color[i] - worst_profit_color[i] for i in range(len(best_profit_color))) * color_interp(item_profit_ratios[item_index])
                else:
                    fill_color = (1, 0.5, 0.5)

                self.show_item(item_index, ax1, boundary_color, fill_color, container_color, show_item_value_and_weight and show_value_and_weight_for_container_items, font, show_bounding_boxes, show_reference_positions)

            # determine the boundary rectangle of the outside-of-container items
            elif show_outside_items and ax2:

                outside_item_bounds[item_index] = get_bounds(self.problem.items[item_index].shape)
                total_outside_item_width += abs(outside_item_bounds[item_index][2] - outside_item_bounds[item_index][0])

        # show the outside-of-container items
        if show_outside_items and ax2:

            out_value_sum = 0
            out_weight_sum = 0
            row_num = max(1, int(np.log10(len(self.problem.items)) * (3 if len(self.problem.items) < 15 else 4)))
            row = 0
            width = 0
            max_width = 0
            row_height = 0
            height = 0
            for item_index, bounds in outside_item_bounds.items():

                out_value_sum += self.problem.items[item_index].value
                out_weight_sum += self.problem.items[item_index].weight

                if color_items_by_profit_ratio:
                    fill_color = worst_profit_color + tuple(best_profit_color[i] - worst_profit_color[i] for i in range(len(best_profit_color))) * color_interp(item_profit_ratios[item_index])
                else:
                    fill_color = (1, 0.5, 0.5)

                min_x, min_y, max_x, max_y = bounds
                shape_width = abs(max_x - min_x)
                shape_height = abs(max_y - min_y)

                shape_center = get_bounding_rectangle_center(self.problem.items[item_index].shape)
                position_offset = (width + shape_width * 0.5 - shape_center[0], row_height + shape_height * 0.5 - shape_center[1])
                self.show_item(item_index, ax2, boundary_color, fill_color, empty_color, show_item_value_and_weight, font, show_bounding_boxes, show_reference_positions, position_offset)

                height = max(height, row_height + shape_height)

                width += shape_width
                max_width += width
                if width >= total_outside_item_width / row_num:
                    row += 1
                    width = 0
                    row_height = height

            # show the value and weight outside the container
            if show_outside_value_and_weight and ax2:
                value_weight_string = "V={}".format(out_value_sum if can_consider_weight else int(out_value_sum))
                if can_consider_weight:
                    value_weight_string += ", W={}".format(out_weight_sum)
                ax2.set_title("Items outside the container\n({})".format(value_weight_string), fontsize=13)

        fig = plt.gcf()

        if show_plot:
            plt.show()

        if save_path:
            fig.savefig(save_path, bbox_inches="tight", dpi=dpi)
            plt.close(fig)

    def show_item(self, item_index, ax, boundary_color, fill_color, container_color, show_item_value_and_weight=False, font=None, show_bounding_box=False, show_reference_position=False, position_offset=(0, 0)):

        """Show the shape of the passed item index in the indicated axis with the passed colors"""

        if item_index in self.placed_items:
            placed_shape = self.placed_items[item_index]
            shape = placed_shape.shape
        else:
            placed_shape = None
            shape = self.problem.items[item_index].shape

        x, y = get_shape_exterior_points(shape, True)
        if position_offset != (0, 0):
            x = [x_i + position_offset[0] for x_i in x]
            y = [y_i + position_offset[1] for y_i in y]

        ax.plot(x, y, color=boundary_color, linewidth=1)
        ax.fill(x, y, color=fill_color)

        if type(shape) == MultiPolygon:
            for geom in shape.geoms:
                for hole in geom.interiors:
                    x, y = get_shape_exterior_points(hole, True)
                    if position_offset != (0, 0):
                        x = [x_i + position_offset[0] for x_i in x]
                        y = [y_i + position_offset[1] for y_i in y]
                    fill_color = container_color
                    boundary_color = (0., 0., 0.)
                    ax.plot(x, y, color=boundary_color, linewidth=1)
                    ax.fill(x, y, color=fill_color)

        # show the value and weight in the centroid if required
        if show_item_value_and_weight and font:
            centroid = get_centroid(shape)
            value = self.problem.items[item_index].value
            if value / int(value) == 1:
                value = int(value)
            weight = self.problem.items[item_index].weight
            if weight / int(weight) == 1:
                weight = int(weight)
            value_weight_string = "v={}\nw={}".format(value, weight)
            item_font = dict(font)
            item_font['size'] = 9
            ax.text(centroid.x + position_offset[0], centroid.y + position_offset[1], value_weight_string, horizontalalignment='center', verticalalignment='center', fontdict=item_font)

        # show the bounding box and its center if needed
        if show_bounding_box:
            bounds = get_bounds(shape)
            min_x, min_y, max_x, max_y = bounds
            x, y = (min_x, max_x, max_x, min_x, min_x), (min_y, min_y, max_y, max_y, min_y)
            if position_offset != (0, 0):
                x = [x_i + position_offset[0] for x_i in x]
                y = [y_i + position_offset[1] for y_i in y]
            boundary_color = (0.5, 0.5, 0.5)
            ax.plot(x, y, color=boundary_color, linewidth=1)
            bounds_center = get_bounding_rectangle_center(shape)
            ax.plot(bounds_center[0] + position_offset[0], bounds_center[1] + position_offset[1], "r.")

        # show the reference position if required
        if show_reference_position and placed_shape:
            ax.plot(placed_shape.position[0], placed_shape.position[1], "b+")


def main():
    """Main function"""
    my_list = [0, 0, 0, 100, 100, 100, 100, 0]
    """
    
    for i in range(1, len(sys.argv)):
        my_list.append(float(sys.argv[i]) * 100)
    """


    all_objects = UnityEngine.Object.FindObjectsOfType(UnityEngine.GameObject)
    for go in all_objects:
        if go.name[-1] != '_' and go.name == "TextTest":
            go.text = "hello"
            go.name = go.name + go.text




    perform_experiments(my_list)


def get_bounds(shape):

    """Return a tuple with the (min_x, min_y, max_x, max_y) describing the bounding box of the shape"""
    return shape.bounds


def get_bounding_rectangle_center(shape):

    """Return the center of the bounding rectangle for the passed shape"""

    return (shape.bounds[0] + shape.bounds[2]) / 2, (shape.bounds[1] + shape.bounds[3]) / 2


def get_centroid(shape):

    """Return the centroid of a shape"""

    return shape.centroid


def get_shape_exterior_points(shape, is_for_visualization=False):

    """Return the exterior points of a shape"""

    if type(shape) == LinearRing:

        return [coord[0] for coord in shape.coords], [coord[1] for coord in shape.coords]

    if type(shape) == MultiPolygon:

        return shape.geoms[0].exterior.xy

    return shape.exterior.xy


def do_shapes_intersect(shape0, shape1):

    """Return whether the two passed shapes intersect with one another"""

    # default case for native-to-native shape test
    return shape0.intersects(shape1)


def get_intersection_points_between_shapes(shape0, shape1):

    """If the two passed shapes intersect in one or more points (a finite number) return all of them, otherwise return an empty list"""

    intersection_points = list()

    # the contour of polygons and multi-polygons is used for the check, to detect boundary intersection points
    if type(shape0) == Polygon:

        shape0 = shape0.exterior

    elif type(shape0) == MultiPolygon:

        shape0 = MultiLineString(shape0.boundary)

    if type(shape1) == Polygon:

        shape1 = shape1.exterior

    elif type(shape1) == MultiPolygon:

        shape1 = MultiLineString(shape1.boundary)

    intersection_result = shape0.intersection(shape1)

    if intersection_result:

        if type(intersection_result) == Point:
            intersection_points.append((intersection_result.x, intersection_result.y))

        elif type(intersection_result) == MultiPoint:
            for point in intersection_result:
                intersection_points.append((point.x, point.y))

    return intersection_points


def does_shape_contain_other(container_shape, content_shape):

    """Return whether the first shape is a container of the second one, which in such case acts as the content of the first one"""

    # use the standard within check
    return content_shape.within(container_shape)


def copy_shape(shape):

    """Create and return a deep copy of the passed shape"""

    return copy.deepcopy(shape)


def get_rectangle_points_from_bounds(min_x, min_y, max_x, max_y):

    """Find and return as (x, y) tuples the rectangle points from the passed bounds"""

    return [(min_x, min_y), (max_x, min_y), (min_x, max_y), (max_x, max_y)]


def create_equilateral_triangle(center, side):

    """Create an equilateral triangle (with conventional orientation) given its center and side length; following the method explained in: https://math.stackexchange.com/a/1344707"""

    root_of_3 = math.sqrt(3)
    vertex0 = (center[0], center[1] + root_of_3 / 3 * side)
    vertex1 = (center[0] - side * 0.5, center[1] - root_of_3 / 6 * side)
    vertex2 = (center[0] + side * 0.5, center[1] - root_of_3 / 6 * side)

    return Polygon([vertex0, vertex1, vertex2])


def create_square(center, side):

    """Create a square (with conventional orientation) given its center and side length"""

    half_side = side * 0.5
    return Polygon([(center[0] - half_side, center[1] - half_side), (center[0] + half_side, center[1] - half_side), (center[0] + half_side, center[1] + half_side), (center[0] - half_side, center[1] + half_side)])


def create_random_triangle_in_rectangle_corner(min_x, min_y, max_x, max_y):

    """Create a triangle that lies in a corner of the rectangle defined by the passed bounds, i.e. one of the vertices of the triangle is one of the points of the rectangle, and the other two vertices lie on sides of the rectangle"""

    # determine the points of the rectangle from its bounds
    rectangle_points = get_rectangle_points_from_bounds(min_x, min_y, max_x, max_y)

    # randomly select a point as one of the vertices of the triangle
    first_vertex = random.choice(rectangle_points)

    # the other two vertices have one (different for each of the two) of the coordinates in common with the first point, and the other two in the range of the bounds
    triangle_points = [first_vertex, (first_vertex[0], random.uniform(min_y, max_y)), (random.uniform(min_x, max_y), first_vertex[1])]

    # create the triangle
    return Polygon(triangle_points)


def create_random_quadrilateral_in_rectangle_corners(min_x, min_y, max_x, max_y):

    """Create a quadrilateral with a side in common with the rectangle defined by the passed bounds; two of the vertices of the quadrilateral coincide with two side-adjacent vertices of the rectangle, and the other two vertices lie on two parallel sides of the rectangle"""

    # first define two points, i.e. a segment, between the minimum and maximum value in one dimension, and random values (within the bounds) for the other dimension;     # then, add the vertices of one of two sides of the rectangle that are not cut by the segment
    has_horizontal_segment = bool(random.getrandbits(1))
    if has_horizontal_segment:
        quadrilateral_points = [(min_x, random.uniform(min_y, max_y)), (max_x, random.uniform(min_y, max_y))]
        if bool(random.getrandbits(1)):
            quadrilateral_points.extend([(max_x, max_y), (min_x, max_y)])
        else:
            quadrilateral_points.extend([(max_x, min_y), (min_x, min_y)])
    else:
        quadrilateral_points = [(random.uniform(min_x, max_x), min_y), (random.uniform(min_x, max_x), max_y)]
        if bool(random.getrandbits(1)):
            quadrilateral_points.extend([(min_x, max_y), (min_x, min_y)])
        else:
            quadrilateral_points.extend([(max_x, max_y), (max_x, min_y)])

    # create the quadrilateral
    return Polygon(quadrilateral_points)


def create_random_polygon(min_x, min_y, max_x, max_y, vertex_num):

    """Create a random polygon with the passed x and y bounds and the passed number of vertices; code adapted from: https://stackoverflow.com/a/45841790"""

    # generate the point coordinates within the bounds
    x = np.random.uniform(min_x, max_x, vertex_num)
    y = np.random.uniform(min_y, max_y, vertex_num)

    # determine the center of all points
    center = (sum(x) / vertex_num, sum(y) / vertex_num)

    # find the angle of each point from the center
    angles = np.arctan2(x - center[0], y - center[1])

    # sort points by their angle from the center to avoid self-intersections
    points_sorted_by_angle = sorted([(i, j, k) for i, j, k in zip(x, y, angles)], key=lambda t: t[2])

    # the process fails if there are duplicate points
    if len(points_sorted_by_angle) != len(set(points_sorted_by_angle)):
        return None

    # structure points as x-y tuples
    points = [(x, y) for (x, y, a) in points_sorted_by_angle]

    # create the polygon
    return Polygon(points)

if __name__ == "__main__":
    main()
