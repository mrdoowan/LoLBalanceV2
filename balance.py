'''
Created by: Steven Duan and Daniel Jin

With the written document, this will balance teams based on a Primary and 
multiple Secondary role preferences

'''

# "Global" variables

# Only to compare Ranks together
COMPARE_RANK = {
	'Bronze 5': 1,
	'Bronze 4': 2,
	'Bronze 3': 3,
	'Bronze 2': 4,
	'Bronze 1': 5,
	'Silver 5': 6,
	'Silver 4': 7,
	'Silver 3': 8,
	'Silver 2': 9,
	'Silver 1': 10,
	'Gold 5': 11,
	'Gold 4': 12,
	'Gold 3': 13,
	'Gold 2': 14,
	'Gold 1': 15,
	'Platinum 5': 16,
	'Platinum 4': 17,
	'Platinum 3': 18, 
	'Platinum 2': 19,
	'Platinum 1': 20,
	'Diamond 5': 21,
	'Diamond 4': 22,
	'Diamond 3': 23,
	'Diamond 2': 24,
	'Diamond 1': 25,
	'Master': 26,
	'Challenger': 27
}

# The translation from a Ranking to a Tier
# Explained in documentation
RANK_TIER = {
	'Bronze 5': 0.5,
	'Bronze 4': 0.5,
	'Bronze 3': 0.5,
	'Bronze 2': 0.5,
	'Bronze 1': 1.0,
	'Silver 5': 1.0,
	'Silver 4': 1.5,
	'Silver 3': 1.5,
	'Silver 2': 2.0,
	'Silver 1': 2.0,
	'Gold 5': 2.5,
	'Gold 4': 2.5,
	'Gold 3': 3.0,
	'Gold 2': 3.0,
	'Gold 1': 3.0,
	'Platinum 5': 3.5,
	'Platinum 4': 3.5,
	'Platinum 3': 4.0, 
	'Platinum 2': 4.0,
	'Platinum 1': 4.0,
	'Diamond 5': 4.5,
	'Diamond 4': 4.5,
	'Diamond 3': 5.0,
	'Diamond 2': 5.0,
	'Diamond 1': 5.0,
	'Master': 5.0,
	'Challenger': 5.0
}

# Map of Duos that will be a global
DUOS_MAP = {}

class Player:
	# Initializer
	def __init__(self, IGN, rank, primary, second, duo):
		self.IGN = IGN				# string
		self.rank = rank 			# int
		self.primary_role = primary # string
		self.second_role = second 	# string[]
		self.duo = duo 				# string
		# add to DUOS_MAP
		DUOS_MAP[IGN] = duo
		# Primary fill should empty out secondary roles
		if (primary == "Fill"):
			self.second_role = []
		# Force fill if primary is in secondary and there's only one secondary
		elif ((primary in second) and (len(second) == 1)):
			self.primary_role = "Fill"
			self.second_role = []
		# If primary is in secondary, but secondary > 1, remove from secondary
		elif ((primary in second) and (len(second) > 1)):
			self.second_role.remove(primary)
		self.tier = RANK_TIER[rank]

	# Only time the below is needed is if I'm using this class as a key to a dictionary
	'''
	def __hash__(self):
		return hash((self.IGN, self.rank, self.primary_role, self.second_role, self.tier))

	def __eq__(self, other):
		return ???

	def __ne__(self, other):
		return ???
	'''

#This is the int main()
if __name__ == "__main__":
	fileName = str(sys.argv[1])
	# Parse fileName of players
	Players = []
	# PARSING HERE (not sure if using .csv, .txt)
	# for ():
	#	addPlayer = Player(class parameters)
	# 	Players.append(addPlayer)

