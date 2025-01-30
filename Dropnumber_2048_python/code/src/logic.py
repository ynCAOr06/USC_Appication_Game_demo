import random
import constants as c

current_con = 0


def new_game():
    matrix = []
    for i in range(c.GRID_HEIGHT):
        matrix.append([0] * c.GRID_LEN)
    matrix = ran_init(matrix)
    return matrix


def ran_init(mat):
    num_list = [2, 4, 8, 16]
    val = random.choice(num_list)
    mat[0][2] = val
    return mat


def eli_check(mat):
    for i in range(len(mat)-1):
        for j in range(len(mat[0])-1):
            if (mat[i][j] == mat[i+1][j] or mat[i][j+1] == mat[i][j]) and mat[i][j] != 0:
                return True
    j = len(mat[0])-1
    for i in range(len(mat)-1):
        if mat[i][j] == mat[i+1][j] and mat[i][j] != 0:
            return True
    i = len(mat)-1
    for j in range(len(mat[0])-1):
        if mat[i][j+1] == mat[i][j] and mat[i][j] != 0:
            return True
    return False


def game_state(mat):
    # check for win cell
    for i in range(len(mat)):
        for j in range(len(mat[0])):
            if mat[i][j] == 2048:
                return 'win'
    # check whether the play area exceeds, if yes, consider grace conditions
    flag = False
    for j in range(c.GRID_LEN):
        if mat[1][j] != 0:
            flag = True
    if not flag:
        return 'not over'
    # if area exceeds, check whether if it is still playable on some point
    flag = False
    for j in range(c.GRID_LEN):
        if mat[1][j] != 0:
            if mat[2][j] != mat[1][j]:
                flag = True
    if not flag:
        return 'not over'
    # or, very trickily, you can generate the above case:
    flag = False
    for j in range(c.GRID_LEN):
        if mat[1][j] != 0:
            if mat[0][j] != mat[1][j]:
                flag = True
    if not flag:
        return 'not over'

    return 'lose'


def move_down(mat, con = None):
    if con is None:
        for j in range(len(mat[0])):
            for i in range(len(mat)-1):
                if mat[i][j] != 0 and mat[i+1][j] == 0:
                    for k in range(i+1):
                        if mat[i-k][j] != 0:
                            mat[i-k][j], mat[i-k+1][j] = mat[i-k+1][j], mat[i-k][j]
    else:
        for i in range(len(mat)-1):
            if mat[i][con] != 0 and mat[i+1][con] == 0:
                for k in range(i+1):
                    if mat[i-k][con] != 0:
                        mat[i-k][con], mat[i-k+1][con] = mat[i-k+1][con], mat[i-k][con]
    return mat


def merge(mat):
    while eli_check(mat):
        # 1st, merge column to reduce the height
        for i in range(len(mat)-1):
            for j in range(len(mat[0])-1):
                if mat[i][j] == mat[i+1][j] and mat[i][j] != 0:
                    mat[i+1][j] *= 2
                    mat[i][j] = 0
                if mat[i][j] == mat[i][j+1] and mat[i][j] != 0:
                    (con_1, con_2) = (j, j+1) if current_con == j+1 else (j+1, j)
                    mat[i][con_1] *= 2
                    mat[i][con_2] = 0
        j = len(mat[0])-1
        for i in range(len(mat) - 1):
            if mat[i][j] == mat[i+1][j] and mat[i][j] != 0:
                mat[i + 1][j] *= 2
                mat[i][j] = 0
        i = len(mat)-1
        for j in range(len(mat[0])-1):
            if mat[i][j] == mat[i][j+1] and mat[i][j] != 0:
                (con_1, con_2) = (j, j + 1) if current_con == j + 1 else (j + 1, j)
                mat[i][con_1] *= 2
                mat[i][con_2] = 0

        mat = move_down(mat)
    return mat


def down(game):
    """ Release the generated block """
    is_down = True
    for i in range(c.GRID_LEN):
        if game[0][i] != 0:
            current_con = i
            game = move_down(game, con = i)

    game = merge(game)
    return game, is_down


def left(game):
    is_down = False
    for i in range(c.GRID_LEN):
        if game[0][i] != 0 and i > 0:
            game[0][i], game[0][i-1] = game[0][i-1], game[0][i]
    return game, is_down


def right(game):
    is_down = False
    for i in range(c.GRID_LEN):
        if game[0][i] != 0 and i < c.GRID_LEN-1:
            game[0][i], game[0][i+1] = game[0][i+1], game[0][i]
            # to avoid loop reassignments
            break
    return game, is_down